﻿// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class Level01 : MonoBehaviour 
	{
		public static string[] Dialog_Start = new string[] 
		{
			"The emergency beacon went off a few hours ago.",
			"But I've never seen this place so quiet.",
			"Not a good sign."
		};

		public static string[] Dialog_OuterDoor = new string[]
		{
			"Huh. The Fuse is good, and Power is on.",
			"I just need to find the reset crank for the Motor."
		};

		public static string[] Dialog_PowerOutage = new string[]
		{
			"A power outage? At CTRL Base Zero? Crap.",
			"There's a backup generator on each floor, including this one.",
			"I need to activate it so I can go deeper."
		};

		enum DialogName { None, Start, OuterDoor, PowerOutage }

		public GameObject _outside;
		public GameObject _inside;
		public GameObject _gradientWalls;

		// once this opens, wait for the door to finish closing to open the inner door
		public HeroTrigger _outerTrigger;
		public DoorStation _outerDoor;
		public DoorStation _innerDoor;

		public AudioSource _openStrings;
		public AudioSource _outdoorAir;
		public AudioSource _indoorAir;

		private float _countdownToOpenInnerDoor = 2.5f;
		private bool _openedInnerDoor;
		private bool _initialized; // late init to avoid conflict with Start scripts
		private DialogName _activeDialog;

		private void HandleAutoTrigger (HeroTriggerType triggerType)
		{
			_outerTrigger.OnAutoTrigger -= HandleAutoTrigger;
			enabled = true;
		}

		private void HandleDialogClose ()
		{
			switch (_activeDialog)
			{
				case DialogName.Start:
					_openStrings.Play();
					break;
			}
			_activeDialog = DialogName.None;
			Dialog.Instance.OnDialogClose -= HandleDialogClose;
		}

		private IEnumerator TriggerPowerOutage ()
		{
			yield return new WaitForSeconds(3);
			PowerStation.Instance.TurnOff();
			yield return new WaitForSeconds(2);
			StartDialog(Dialog_PowerOutage, DialogName.PowerOutage);
			yield return null;
		}

		private void StartDialog (string[] lines, DialogName dialogName)
		{
			Dialog.Instance.ShowBaked(lines);
			_activeDialog = dialogName;
			Dialog.Instance.OnDialogClose += HandleDialogClose;
		}

		protected void Awake ()
		{
			_gradientWalls.SetActive(true);
		}

		protected void Update ()
		{
			if (_initialized == false)
			{
				StartDialog(Dialog_Start, DialogName.Start);
				_inside.SetActive(false);
				_outerTrigger.OnAutoTrigger += HandleAutoTrigger;
				enabled = false;
				_initialized = true;
				return;
			}

			if (_outerDoor.IsClosed == false) return;
			_outside.SetActive(false);

			_outdoorAir.Stop();

			if (_openedInnerDoor == false)
			{
				_countdownToOpenInnerDoor -= Time.deltaTime;

				if (_countdownToOpenInnerDoor <= 0)
				{
					_openedInnerDoor = true;
					_innerDoor.Open();
					_inside.SetActive(true);
				}
			}
			else
			{
				if (_innerDoor.CurrentState == DoorGadget.State.Opening)
				{
					_indoorAir.Play();
					StartCoroutine(TriggerPowerOutage());
					enabled = false;
				}
			}
		}
	}
}
