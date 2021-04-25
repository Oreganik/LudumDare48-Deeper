// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class ElevatorDoor : MonoBehaviour 
	{
		const float OPEN_TIME = 2;
		const float CLOSE_TIME = 3;
		const float DISTANCE = 0.96f;

		public enum State { Closed, Opening, Open, Closing }

		public State CurrentState { get{ return _state; }}

		public bool IsOpen
		{
			get { return _state == State.Open; }
		}

		public bool IsClosed
		{
			get { return _state == State.Closed; }
		}

		public int _direction = 1;

		private float _timer;
		private State _state;
		private Vector3 _closedPosition;
		private Vector3 _openPosition;

		public void Close ()
		{
			if (_state != State.Open) 
			{
				Debug.Log("Can't close: state is " + _state);
				return;
			}
			Debug.Log("Close");
			enabled = true;
			_state = State.Closing;
			_timer = 0;
		}

		public void Open ()
		{
			if (_state != State.Closed)
			{
				Debug.Log("Can't open: state is " + _state);
				return;
			}
			Debug.Log("Open");
			enabled = true;
			_state = State.Opening;
			_timer = 0;
		}

		protected void Awake ()
		{
			_closedPosition = transform.position;
			_openPosition = _closedPosition + transform.right * _direction * DISTANCE;
			enabled = false;
		}

		protected void Update ()
		{
			_timer += Time.deltaTime;

			float duration = OPEN_TIME;
			Vector3 start = _closedPosition;
			Vector3 end = _openPosition;

			if (_state == State.Closing)
			{
				duration = CLOSE_TIME;
				start = _openPosition;
				end = _closedPosition;
			}

			float t = Mathf.Clamp01(_timer / duration);

			transform.position = Vector3.Lerp(start, end, t);

			if (t >= 1)
			{
				Debug.Log("Finished " + _state);
				if (_state == State.Opening) 
				{
					_state = State.Open;
				}
				else 
				{
					_state = State.Closed;
				}
				enabled = false;
			}
		}
	}
}
