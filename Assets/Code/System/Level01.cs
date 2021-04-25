// LUDUM DARE 48: DEEPER AND DEEPER
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
			"I've never seen this place so quiet.",
			"Not a good sign."
		};

		public static string[] Dialog_OuterDoor = new string[]
		{
			"Interesting.",
			"Good fuse, steady power, but the door motor is off.",
			"If there's a crank nearby, I can manually restart it."
		};

		public static string[] Dialog_PowerOutage = new string[]
		{
			"A power outage? At CTRL Base Zero? Crap.",
			"There's a backup generator on each floor, including this one.",
			"I need to activate it so I can go deeper."
		};

		public static string[] Dialog_Elevator = new string[]
		{
			"I hate this place."
		};

		public enum DialogName { None, Start, OuterDoor, PowerOutage, Elevator }

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
		public HeroTrigger _triggerPowerOutage;

		public Elevator _levelEndElevator;
		public Transform _elevatorLookTarget;

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
			PowerStation.Instance.TurnOff();
			yield return new WaitForSeconds(2);
			StartDialog(DialogName.PowerOutage);
			yield return null;
		}

		private IEnumerator LoadNextLevelAfterElevatorCloses ()
		{
			Hero.Instance.SetDialogLookTarget(_elevatorLookTarget.position);
			yield return new WaitForSeconds(1);
			StartDialog(DialogName.Elevator);
			while (_levelEndElevator.IsClosed == false)
			{
				yield return null;
			}
			yield return new WaitForSeconds(1);
			Session.LoadNextLevel();
		}

		public void StartDialog (DialogName dialogName)
		{
			switch (dialogName)
			{
				case DialogName.Elevator:
					Dialog.Instance.ShowBaked(Dialog_Elevator);
					break;
				case DialogName.OuterDoor:
					Dialog.Instance.ShowBaked(Dialog_OuterDoor);
					break;
				case DialogName.PowerOutage:
					Dialog.Instance.ShowBaked(Dialog_PowerOutage);
					break;
				case DialogName.Start:
					Dialog.Instance.ShowBaked(Dialog_Start);
					break;
			}
			_activeDialog = dialogName;
			Dialog.Instance.OnDialogClose += HandleDialogClose;
		}

		private void HandleActivateElevator ()
		{
			StartCoroutine(LoadNextLevelAfterElevatorCloses());
		}

		private void HandleAutoTriggerPowerOutage (HeroTriggerType triggerType)
		{
			_triggerPowerOutage.OnAutoTrigger -= HandleAutoTriggerPowerOutage;
			StartCoroutine(TriggerPowerOutage());
		}

		protected void Awake ()
		{
			_gradientWalls.SetActive(true);
			_levelEndElevator.OnActivateElevator += HandleActivateElevator;
			_triggerPowerOutage.OnAutoTrigger += HandleAutoTriggerPowerOutage;
		}

		protected void Update ()
		{
			if (_initialized == false)
			{
				StartDialog(DialogName.Start);
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
					enabled = false;
				}
			}
		}
	}
}
