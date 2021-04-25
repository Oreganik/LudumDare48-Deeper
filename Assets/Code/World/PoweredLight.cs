// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class PoweredLight : MonoBehaviour 
	{
		public bool IsEmergencyLight;
		public bool IsGadgetControlled;

		public Renderer _renderer;
		public Material _lightOn;
		public Material _lightOff;

		private Light _light;

		public void SetLit (bool value, bool immediately = false)
		{
			if (_light)
			{
				_light.gameObject.SetActive(value);
			}
			if (_renderer)
			{
				_renderer.sharedMaterial = value ? _lightOn : _lightOff;
			}
		}

		protected void Awake ()
		{
			_light = GetComponentInChildren<Light>();
		}

		protected void Start ()
		{
			if (PowerStation.Instance && !IsGadgetControlled)
			{
				PowerStation.Instance.RegisterPoweredLight(this);
			}
		}
	}
}
