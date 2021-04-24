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
		public DoorGadget _door0;
		public DoorGadget _door1;
		public TMPro.TMP_Text _debugText;
		public FuseStation _fuseStation;

		private void HandleStartInteract (HeroTriggerType triggerType)
		{
			if (triggerType != HeroTriggerType.Door)
			{
				Debug.LogError("Weird: " + this.GetType().ToString() + " received HeroTriggerType." + triggerType.ToString());
				return;
			}

			if (_fuseStation && _fuseStation.IsWorking == false)
			{
				Debug.Log("Target fuse station " + _fuseStation.name + " is not working");
				return;
			}

			if (PowerStation.Instance.HasPower == false)
			{
				Debug.Log("Power Station does not have power");
				return;
			}

			if (_door0.CurrentState == DoorGadget.State.Closed)
			{
				_door0.Open();
				_door1.Open();
			}
			else if (_door0.CurrentState == DoorGadget.State.Open)
			{
				_door0.Close();
				_door1.Close();
			}
			else
			{
				Debug.Log("Door is in State." + _door0.CurrentState);
			}
		}

		protected void Awake ()
		{
			GetComponentInChildren<HeroTrigger>().OnStartInteract += HandleStartInteract;
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
		}
	}
}
