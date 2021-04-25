// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class PowerStation : MonoBehaviour 
	{
		public static PowerStation Instance;

		public bool HasPower { get; private set; }

		public bool _startsPowered = true;
		public TMPro.TMP_Text _debugText;
		public AudioSource _soundOff;
		public AudioSource _soundOn;

		private List<PoweredLight> _poweredLights;
		private bool _initialized;
		private bool _showInstructions;

		public void RegisterPoweredLight (PoweredLight light)
		{
			_poweredLights.Add(light);
			if (light.IsEmergencyLight) light.SetLit(!HasPower, immediately: true);
			else light.SetLit(HasPower, immediately: true);
		}

		public void TogglePower ()
		{
			if (HasPower) TurnOff();
			else TurnOn();
		}

		public void TurnOff (bool playSound = true)
		{
			Debug.Log("Turn off power");
			HasPower = false;

			foreach (PoweredLight light in _poweredLights)
			{
				if (light.IsEmergencyLight) light.SetLit(!HasPower);
				else light.SetLit(HasPower);
			}

			if (playSound) _soundOff.Play();
		}

		public void TurnOn (bool playSound = true)
		{
			Debug.Log("Turn on power");
			HasPower = true;

			foreach (PoweredLight light in _poweredLights)
			{
				if (light.IsEmergencyLight) light.SetLit(!HasPower);
				else light.SetLit(HasPower);
			}

			if (playSound) _soundOn.Play();
		}

		private void HandleHeroEnterTrigger (HeroTriggerType triggerType)
		{
			if (triggerType != HeroTriggerType.Power)
			{
				Debug.LogError("Weird: " + this.GetType().ToString() + " received HeroTriggerType." + triggerType.ToString());
				return;
			}
			_showInstructions = true;
		}

		private void HandleHeroExitTrigger (HeroTriggerType triggerType)
		{
			if (triggerType != HeroTriggerType.Power)
			{
				Debug.LogError("Weird: " + this.GetType().ToString() + " received HeroTriggerType." + triggerType.ToString());
				return;
			}
			Instructions.Instance.Hide();
			_showInstructions = false;
		}

		private void HandleHeroInteract (HeroTriggerType triggerType)
		{
			if (triggerType != HeroTriggerType.Power)
			{
				Debug.LogError("Weird: " + this.GetType().ToString() + " received HeroTriggerType." + triggerType.ToString());
				return;
			}
			TogglePower();
		}

		protected void Awake ()
		{
			Instance = this;
			_poweredLights = new List<PoweredLight>();
			HeroTrigger _trigger = GetComponentInChildren<HeroTrigger>();
			_trigger.OnStartInteract += HandleHeroInteract;
			_trigger.OnHeroEnterTrigger += HandleHeroEnterTrigger;
			_trigger.OnHeroExitTrigger += HandleHeroExitTrigger;
		}

		protected void Update ()
		{
			// lets lights register on start
			if (_initialized == false)
			{
				if (_startsPowered)
				{
					TurnOn(playSound: false);
				}
				else
				{
					TurnOff(playSound: false);
				}
				_initialized = true;
			}
			
			if (Session.DebugVisible)
			{
				_debugText.enabled = true;
				_debugText.text = "Power: " + (HasPower ? "On" : "Off");
			}
			else
			{
				_debugText.enabled = false;
			}

			if (_showInstructions)
			{
				if (HasPower)
				{
					Instructions.Instance.Show("Generator", "Left click or E to turn off");
				}
				else
				{
					Instructions.Instance.Show("Generator", "Left click or E to turn on");
				}
			}
		}
	}
}
