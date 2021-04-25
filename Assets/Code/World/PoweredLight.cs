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

		public void SetLit (bool value, bool immediately = false)
		{
			gameObject.SetActive(value);
		}

		protected void Start ()
		{
			if (PowerStation.Instance)
			{
				PowerStation.Instance.RegisterPoweredLight(this);
			}
		}
	}
}
