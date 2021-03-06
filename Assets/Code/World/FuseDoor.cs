// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	// NOTE: DOOR MESH MUST START OPEN FOR THIS TO WORK VISUALLY
	public class FuseDoor : MonoBehaviour 
	{
		const float OPEN_DURATION = 1f;
		const float CLOSE_DURATION = 0.5f;
		enum State { Open, Closing, Closed, Opening }
		public bool IsOpen { get { return _state == State.Open; } }
		public bool IsClosed { get { return _state == State.Closed; } }
		public bool IsMoving { get { return _state == State.Opening || _state == State.Closing; } }

		public AudioSource _fuseDoorOpen;
		public AudioSource _fuseDoorClose;

		private float _rotation = 0;
		private float _speed = 0;
		private State _state;

		public void Close (bool immediately = false)
		{
			if (immediately)
			{
				SetRotation(180);
				_speed = 0;
			}
			else
			{
				_speed = 180 / CLOSE_DURATION;
				_state = State.Closing;
				if (immediately == false) _fuseDoorClose.Play();
			}
		}

		public void Open (bool immediately = false)
		{
			if (immediately)
			{
				SetRotation(0);
				_speed = 0;
			}
			else
			{
				_speed = -180 / OPEN_DURATION;
				_state = State.Opening;
				if (immediately == false) _fuseDoorOpen.Play();
			}
		}

		private void SetRotation (float value)
		{
			_rotation = Mathf.Clamp(value, 0, 180);
			transform.localRotation = Quaternion.Euler(new Vector3(-90, _rotation, 0));
			if (_rotation <= 0) _state = State.Open;
			if (_rotation >= 180) _state = State.Closed;
		}

		protected void Update ()
		{
			SetRotation(_rotation + _speed * Time.deltaTime);
		}
	}
}
