// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class Chub : MonoBehaviour 
	{
		enum State { Move, WaitToTurn, Turn, WaitToMove, Unpowered }

		public const float MOVE_SPEED = 1;
		public const float ROTATE_DURATION = 1; // time to rotate 90 degrees
		public const float WAIT_DURATION = 2;
		public const float DISTANCE_PER_TILE = 2;

		public GameObject _fusePrefab;
		public int _sideTiles = 2;
		public int _forwardTiles = 2;
		public bool _turnRight;
		public bool _disabled;
		public bool _startWithFuse = true;
		public Transform _fuseLocation;
		
		private bool _goForward = true;
		private float _timer = 0;
		private State _state;
		private Vector3 _targetPosition;
		private Quaternion _targetRotation;
		private float _rotation = 0;
		private GameObject _fuseObject;
		private bool _showInstructions;

		public void FallOver ()
		{
			Rigidbody rb = GetComponent<Rigidbody>();
			rb.isKinematic = false;
			rb.useGravity = true;
			rb.constraints = RigidbodyConstraints.None;
			rb.angularVelocity = UnityEngine.Random.insideUnitSphere * 10;
			_disabled = true;
		}

		private void HandleHeroEnterTrigger (HeroTriggerType triggerType)
		{
			_showInstructions = true;
		}

		private void HandleHeroExitTrigger (HeroTriggerType triggerType)
		{
			Instructions.Instance.Hide();
			_showInstructions = false;
		}

		private void HandleStartInteract (HeroTriggerType triggerType)
		{
			if (triggerType != HeroTriggerType.PullFuseFromRobot)
			{
				Debug.LogError("Weird: " + this.GetType().ToString() + " received HeroTriggerType." + triggerType.ToString());
				return;
			}

			if (_fuseObject == null)
			{
				Debug.Log("fuse object is null");
				return;
			}

			Hero.Instance.AddFuse();
			if (_fuseObject)
			{
				Destroy(_fuseObject);
				_fuseObject = null;
			}
			FallOver();
			Instructions.Instance.Hide();
			_showInstructions = false;
		}

		protected void Awake ()
		{
			_state = State.WaitToMove;
			_rotation = transform.rotation.eulerAngles.y;

			if (_startWithFuse)
			{
				_fuseObject = Instantiate(_fusePrefab);
				_fuseObject.transform.parent = transform;
				_fuseObject.transform.position = _fuseLocation.position;
				_fuseObject.transform.rotation = _fuseLocation.rotation;
				_fuseObject.transform.Rotate(Vector3.right * 90, Space.Self);

				HeroTrigger trigger = GetComponentInChildren<HeroTrigger>();
				trigger.OnStartInteract += HandleStartInteract;
				trigger.OnHeroEnterTrigger += HandleHeroEnterTrigger;
				trigger.OnHeroExitTrigger += HandleHeroExitTrigger;
			}

			_fuseLocation.gameObject.SetActive(false);

			if (_disabled)
			{
				FallOver();
			}
		}

		protected void Update ()
		{
			if (_disabled) return;

			if (_showInstructions && _fuseObject)
			{
				Instructions.Instance.Show("Fuse", "Left click or E to pull fuse");
			}

			float t = 0;
			switch (_state)
			{
				case State.Move:
					transform.position = Vector3.MoveTowards(transform.position, _targetPosition, MOVE_SPEED * Time.deltaTime);
					if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
					{
						transform.position = _targetPosition;
						_state = State.WaitToTurn;
						_timer = 0;
					}
					break;

				case State.WaitToTurn:
					_timer += Time.deltaTime;
					if (_timer > WAIT_DURATION)
					{
						_state = State.Turn;
						if (_turnRight) _rotation += 90;
						else _rotation -= 90;
						_targetRotation = Quaternion.Euler(Vector3.up * _rotation);
						_timer = 0;
					}
					break;

				case State.Turn:
					_timer += Time.deltaTime;
					t = Mathf.Clamp01(_timer / ROTATE_DURATION);
					transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, t);
					if (_timer >= ROTATE_DURATION)
					{
						transform.rotation = _targetRotation;
						_goForward = !_goForward;
						_state = State.WaitToMove;
						_timer = 0;
					}
					break;

				case State.WaitToMove:
					_timer += Time.deltaTime;
					if (_timer > WAIT_DURATION)
					{
						_targetPosition = transform.position + transform.forward * DISTANCE_PER_TILE * (_goForward ? _forwardTiles : _sideTiles);
						_state = State.Move;
						_timer = 0;
					}
					break;
			}
		}
	}
}
