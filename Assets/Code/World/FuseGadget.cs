// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class FuseGadget : MonoBehaviour 
	{
		public bool IsOpen { get { return _door.IsOpen; } }
		public FuseState State { get; private set; }
		public FuseDoor Door { get { return _door; } }

		public GameObject _badFuse;
		public GameObject _goodFuse;
		public HeroTrigger _trigger;

		private FuseDoor _door;

		public void SetFuseState (FuseState newState)
		{
			State = newState;
			_badFuse.SetActive(newState == FuseState.Bad);
			_goodFuse.SetActive(newState == FuseState.Good);
		}

		private void HandleStartInteract (HeroTriggerType triggerType)
		{
			if (triggerType != HeroTriggerType.Fuse)
			{
				Debug.LogError("Weird: " + this.GetType().ToString() + " received HeroTriggerType." + triggerType.ToString());
				return;
			}
			
			// Do nothing if the door is moving
			if (_door.IsMoving)
			{
				return;
			}

			// If the door is open, either interact with the fuse or close the box
			if (IsOpen)
			{
				switch (State)
				{
					case FuseState.Bad:
						SetFuseState(FuseState.Empty);
						if (PowerStation.Instance.HasPower)
						{
							Hero.Instance.Die();
						}
						break;

					case FuseState.Empty:
						SetFuseState(FuseState.Good);
						if (PowerStation.Instance.HasPower)
						{
							Hero.Instance.Die();
						}
						break;

					case FuseState.Good:
						_door.Close();
						break;
				}
			}
			// If the door is closed, and the fuse is not good, open it
			else
			{
				if (State != FuseState.Good)
				{
					_door.Open();
				}
			}
		}

		protected void Awake ()
		{
			_door = GetComponentInChildren<FuseDoor>();
			_trigger.OnStartInteract += HandleStartInteract;
		}
	}
}
