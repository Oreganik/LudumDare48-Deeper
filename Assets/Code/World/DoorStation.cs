// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class DoorStation : MonoBehaviour 
	{
		public bool IsClosed
		{
			get { return _door0.CurrentState == DoorGadget.State.Closed; }
		}

		public DoorGadget.State CurrentState
		{
			get { return _door0.CurrentState; }
		}

		public bool HasFuse
		{
			get 
			{
				if (_fuseStation == null) return true;
				return _fuseStation.IsFuseReady;
			}
		}

		public bool HasMotor
		{
			get 
			{
				if (_fuseStation == null) return true;
				return _fuseStation.IsMotorReady;
			}
		}

		public bool HasPower
		{
			get 
			{ 
				if (PowerStation.Instance == null) return true;
				return PowerStation.Instance.HasPower;
			}
		}

		public bool CanBeClosed
		{
			get { return _door0.CurrentState == DoorGadget.State.Open; }
		}

		public bool CanBeOpened
		{
			get { return _door0.CurrentState == DoorGadget.State.Closed; }
		}
		
		public DoorGadget _door0;
		public DoorGadget _door1;
		public TMPro.TMP_Text _debugText;
		public FuseStation _fuseStation;
		public HeroTrigger _doorTrigger;
		public AudioSource _openAudio;
		public AudioSource _closeAudio;

		private bool _showInstructions;

		public void Close ()
		{
			if (_door0.Close())
			{
				_closeAudio.Play();
			}
			_door1.Close();
		}

		public void Open ()
		{
			if (_door0.Open())
			{
				_openAudio.Play();
			}
			_door1.Open();
		}

		private void HandleHeroEnterTrigger (HeroTriggerType triggerType)
		{
			if (triggerType != HeroTriggerType.Door)
			{
				Debug.LogError("Weird: " + this.GetType().ToString() + " received HeroTriggerType." + triggerType.ToString());
				return;
			}
			_showInstructions = true;
		}

		private void HandleHeroExitTrigger (HeroTriggerType triggerType)
		{
			if (triggerType != HeroTriggerType.Door)
			{
				Debug.LogError("Weird: " + this.GetType().ToString() + " received HeroTriggerType." + triggerType.ToString());
				return;
			}
			Instructions.Instance.Hide();
			_showInstructions = false;
		}

		private void HandleStartInteract (HeroTriggerType triggerType)
		{
			if (triggerType != HeroTriggerType.Door)
			{
				Debug.LogError("Weird: " + this.GetType().ToString() + " received HeroTriggerType." + triggerType.ToString());
				return;
			}

			if (HasFuse == false)
			{
				Debug.Log("Target fuse station " + _fuseStation.name + " is not working");
				return;
			}

			if (HasMotor == false)
			{
				Debug.Log("Target fuse station motor " + _fuseStation.name + " is not working");
				return;
			}

			if (HasPower == false)
			{
				Debug.Log("Power Station does not have power");
				return;
			}

			if (CanBeOpened)
			{
				Open();
			}
			else if (CanBeClosed)
			{
				Close();
			}
			else
			{
				Debug.Log("Door is in State." + _door0.CurrentState);
			}
		}

		protected void Awake ()
		{
			_doorTrigger.OnStartInteract += HandleStartInteract;
			_doorTrigger.OnHeroEnterTrigger += HandleHeroEnterTrigger;
			_doorTrigger.OnHeroExitTrigger += HandleHeroExitTrigger;
		}

		protected void Update ()
		{
			if (Session.DebugVisible)
			{
				_debugText.enabled = true;
				_debugText.text = _door0.CurrentState.ToString();
			}
			else
			{
				_debugText.enabled = false;
			}

			if (_showInstructions)
			{
				if (HasFuse == false)
				{
					Instructions.Instance.Show("Security Door", "Disabled: Bad fuse detected");
				}
				else if (HasPower == false)
				{
					Instructions.Instance.Show("Security Door", "Disabled: Power source inactive");
				}
				else if (HasMotor == false)
				{
					Instructions.Instance.Show("Security Door", "Disabled: Motor must be cranked");
				}
				else if (CanBeOpened)
				{
					Instructions.Instance.Show("Security Door", "Left mouse button or E to open");
				}
				else if (CanBeClosed)
				{
					Instructions.Instance.Show("Security Door", "Left mouse button or E to close");
				}
				else
				{
					Instructions.Instance.Hide();
				}
			}
		}
	}
}
