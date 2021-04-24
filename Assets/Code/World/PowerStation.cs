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

		private List<EmergencyLight> _emergencyLights;
		private List<PoweredLight> _poweredLights;
		private bool _initialized;

		public void RegisterEmergencyLight (EmergencyLight light)
		{
			_emergencyLights.Add(light);
		}

		public void RegisterPoweredLight (PoweredLight light)
		{
			_poweredLights.Add(light);
		}

		public void TogglePower ()
		{
			if (HasPower) TurnOff();
			else TurnOn();
		}

		public void TurnOff ()
		{
			Debug.Log("Turn off power");
			HasPower = false;

			foreach (EmergencyLight light in _emergencyLights)
			{
				light.gameObject.SetActive(!HasPower);
			}

			foreach (PoweredLight light in _poweredLights)
			{
				light.gameObject.SetActive(HasPower);
			}
		}

		public void TurnOn ()
		{
			Debug.Log("Turn on power");
			HasPower = true;

			foreach (EmergencyLight light in _emergencyLights)
			{
				light.gameObject.SetActive(!HasPower);
			}

			foreach (PoweredLight light in _poweredLights)
			{
				light.gameObject.SetActive(HasPower);
			}
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
			_emergencyLights = new List<EmergencyLight>();
			_poweredLights = new List<PoweredLight>();
			GetComponentInChildren<HeroTrigger>().OnStartInteract += HandleHeroInteract;
		}

		protected void Update ()
		{
			// lets lights register on start
			if (_initialized == false)
			{
				if (_startsPowered)
				{
					TurnOn();
				}
				else
				{
					TurnOff();
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
		}
	}
}
