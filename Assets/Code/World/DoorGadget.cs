// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class DoorGadget : MonoBehaviour 
	{
		const float UNLOCK_DURATION = 1;
		const float GEAR_ROTATION = 270;
		const float WAIT_DURATION = 1;
		const float OPEN_DURATION = 2;
		const float CLOSE_DURATION = 2f;
		const float OPEN_DISTANCE = 1.3f;

		public enum State { Closed, Unlocking, WaitToOpen, Opening, Open, Closing, Locking }
		
		public State CurrentState
		{
			get { return _state; }
		}

		public Transform _frontAxis;
		public Transform _backAxis;
		public int _openDirection = 1;

		private State _state;
		private float _timer;
		private Vector3 _closePosition;
		private Vector3 _openPosition;

		public bool Close ()
		{
			if (_state != State.Open)
			{
				Debug.LogError("Can't close door " + gameObject.name + " while in State." + _state, gameObject);
				return false;
			}
			GoToState(State.Closing);
			return true;
		}

		public bool Open ()
		{
			if (_state != State.Closed)
			{
				Debug.LogError("Can't open door " + gameObject.name + " while in State." + _state, gameObject);
				return false;
			}
			GoToState(State.Unlocking);
			return true;
		}

		private void GoToState (State nextState)
		{
			_timer = 0;
			_state = nextState;
		}

		protected void Awake ()
		{
			_closePosition = transform.localPosition;
			_openPosition = _closePosition + Vector3.right * OPEN_DISTANCE * _openDirection;
			_frontAxis.localRotation = Quaternion.Euler(Vector3.forward * GEAR_ROTATION);
			_backAxis.localRotation = Quaternion.Euler(Vector3.back * GEAR_ROTATION);
		}

		protected void Update ()
		{
			if (_state == State.Open) return;
			if (_state == State.Closed) return;

			_timer += Time.deltaTime;
			float t = 0;
			float rotation = 0;

			switch (_state)
			{
				case State.Unlocking:
					t = Mathf.Clamp01(_timer / UNLOCK_DURATION);
					rotation = (1 - t) * GEAR_ROTATION;
					_frontAxis.localRotation = Quaternion.Euler(Vector3.forward * rotation);
					_backAxis.localRotation = Quaternion.Euler(Vector3.back * rotation);
					if (t >= 1) 
					{
						GoToState(State.WaitToOpen);
					}
					break;

				case State.WaitToOpen:
					t = Mathf.Clamp01(_timer / WAIT_DURATION);
					if (t >= 1) 
					{
						GoToState(State.Opening);
					}
					break;

				case State.Opening:
					t = Mathf.Clamp01(_timer / OPEN_DURATION);
					transform.localPosition = Vector3.Lerp(_closePosition, _openPosition, t);
					if (t >= 1) 
					{
						GoToState(State.Open);
					}
					break;

				case State.Closing:
					t = Mathf.Clamp01(_timer / CLOSE_DURATION);
					transform.localPosition = Vector3.Lerp(_openPosition, _closePosition, t);
					if (t >= 1) 
					{
						GoToState(State.Locking);
					}
					break;

				case State.Locking:
					t = Mathf.Clamp01(_timer / UNLOCK_DURATION);
					rotation = t * GEAR_ROTATION;
					_frontAxis.localRotation = Quaternion.Euler(Vector3.forward * rotation);
					_backAxis.localRotation = Quaternion.Euler(Vector3.back * rotation);
					if (t >= 1) 
					{
						GoToState(State.Closed);
					}
					break;
			}
		}
	}
}
