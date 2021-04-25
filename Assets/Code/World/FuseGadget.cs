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

		public HeroTrigger _trigger;
		public GameObject _fuseBadPrefab;
		public GameObject _fuseGoodPrefab;
		public Transform _fuseLocation;
		public AudioSource _fuseRemove;
		public PoweredLight _poweredLight;

		private FuseDoor _door;

		private GameObject _goodFuse;
		private GameObject _badFuse;
		private bool _showInstructions;

		public void SetFuseState (FuseState newState, bool immediately = false)
		{
			State = newState;
			_badFuse.SetActive(newState == FuseState.Bad);
			_goodFuse.SetActive(newState == FuseState.Good);
			_poweredLight.SetLit((newState == FuseState.Good), immediately);
			if (newState == FuseState.Good) 
			{
				_door.Close(immediately);
			}
			else if (newState == FuseState.Empty)
			{
				if (immediately == false) _fuseRemove.Play();
			}
		}

		private void HandleHeroEnterTrigger (HeroTriggerType triggerType)
		{
			if (triggerType != HeroTriggerType.Fuse)
			{
				Debug.LogError("Weird: " + this.GetType().ToString() + " received HeroTriggerType." + triggerType.ToString());
				return;
			}
			_showInstructions = true;
		}

		private void HandleHeroExitTrigger (HeroTriggerType triggerType)
		{
			if (triggerType != HeroTriggerType.Fuse)
			{
				Debug.LogError("Weird: " + this.GetType().ToString() + " received HeroTriggerType." + triggerType.ToString());
				return;
			}
			Instructions.Instance.Hide();
			_showInstructions = false;
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
				else
				{
					Instructions.Instance.Hide();
				}
			}
		}

		protected void Awake ()
		{
			_door = GetComponentInChildren<FuseDoor>();
			_trigger.OnStartInteract += HandleStartInteract;
			_trigger.OnHeroEnterTrigger += HandleHeroEnterTrigger;
			_trigger.OnHeroExitTrigger += HandleHeroExitTrigger;

			_badFuse = Instantiate(_fuseBadPrefab);
			_badFuse.transform.parent = _fuseLocation.transform;
			_badFuse.transform.localPosition = Vector3.zero;
			_badFuse.transform.localRotation = Quaternion.Euler(Vector3.right * 90);

			_goodFuse = Instantiate(_fuseGoodPrefab);
			_goodFuse.transform.parent = _fuseLocation.transform;
			_goodFuse.transform.localPosition = Vector3.zero;
			_goodFuse.transform.localRotation = Quaternion.Euler(Vector3.right * 90);

			_fuseLocation.GetComponent<Renderer>().enabled = false;
		}

		protected void Update ()
		{
			if (_showInstructions)
			{
				if (IsOpen)
				{
					switch (State)
					{
						case FuseState.Bad:
							Instructions.Instance.Show("FUSE STATION", "Left click or E to remove bad fuse");
							break;

						case FuseState.Empty:
							Instructions.Instance.Show("FUSE STATION", "Left click or E to insert good fuse");
							break;

						case FuseState.Good:
							Instructions.Instance.Show("FUSE STATION", "Left click or E to close fuse box");
							break;

						
					}
				}
				// If the door is closed, and the fuse is not good, open it
				else
				{
					if (State != FuseState.Good)
					{
						Instructions.Instance.Show("FUSE STATION", "Left click or E to open fuse box");
					}
				}
			}
		}
	}
}
