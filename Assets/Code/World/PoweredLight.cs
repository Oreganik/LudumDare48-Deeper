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

		private Light _light;

		public void SetLit (bool value, bool immediately = false)
		{
			_light.gameObject.SetActive(value);
		}

		protected void Start ()
		{
			_light = GetComponentInChildren<Light>();
			if (PowerStation.Instance)
			{
				PowerStation.Instance.RegisterPoweredLight(this);
			}
		}
	}
}
