// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class Elevator : MonoBehaviour 
	{
		public Action OnActivateElevator;

		public bool IsOpen { get { return _doors[0].IsOpen; } }
		public bool IsClosed { get { return _doors[0].IsClosed; } }

		public ElevatorDoor[] _doors;
		public AudioSource _closeSound;
		public AudioSource _openSound;
		public Collider _doorCollider;
		public HeroTrigger _elevatorTrigger;
		public HeroTrigger _autoOpenTrigger;

		private bool _showInstructions;

		public bool Close ()
		{
			if (_doors[0].IsOpen == false) 
			{
				Debug.Log("Can't Close: Current state is " + _doors[0].CurrentState);
				return false;
			}
			foreach (ElevatorDoor door in _doors)
			{
				door.Close();
			}
			_doorCollider.enabled = true;
			if (_closeSound) _closeSound.Play();
			return true;
		}

		public void Open ()
		{
			if (_doors[0].IsClosed == false) 
			{
				Debug.Log("can't open door");
				return;
			}
			foreach (ElevatorDoor door in _doors)
			{
				door.Open();
			}
			_doorCollider.enabled = false;
			if (_openSound) _openSound.Play();
		}

		private void HandleAutoTrigger (HeroTriggerType triggerType)
		{
			Debug.Log("handle auto trigger");
			Open();
			_autoOpenTrigger.OnAutoTrigger -= HandleAutoTrigger;
		}

		private void HandleHeroEnterTrigger (HeroTriggerType triggerType)
		{
			if (triggerType != HeroTriggerType.Elevator)
			{
				Debug.LogError("Weird: " + this.GetType().ToString() + " received HeroTriggerType." + triggerType.ToString());
				return;
			}
			_showInstructions = true;
		}

		private void HandleHeroExitTrigger (HeroTriggerType triggerType)
		{
			if (triggerType != HeroTriggerType.Elevator)
			{
				Debug.LogError("Weird: " + this.GetType().ToString() + " received HeroTriggerType." + triggerType.ToString());
				return;
			}
			Instructions.Instance.Hide();
			_showInstructions = false;
		}

		private void HandleStartInteract (HeroTriggerType triggerType)
		{
			Debug.Log("handle start interact");
			if (Close())
			{
				Instructions.Instance.Hide();
				if (OnActivateElevator != null)
				{
					OnActivateElevator();
				}
			}
		}

		protected void Awake ()
		{
			_elevatorTrigger.OnHeroEnterTrigger += HandleHeroEnterTrigger;
			_elevatorTrigger.OnHeroExitTrigger += HandleHeroExitTrigger;
			_elevatorTrigger.OnStartInteract += HandleStartInteract;
			if (_autoOpenTrigger != null)
			{
				_autoOpenTrigger.OnAutoTrigger += HandleAutoTrigger;
			}
		}

		protected void Update ()
		{
			if (_showInstructions)
			{
				if (IsOpen)
				{
					Instructions.Instance.Show("Elevator", "Left click or E to descend");
				}
			}
		}
	}
}
