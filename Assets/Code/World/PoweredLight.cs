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
		protected void Start ()
		{
			PowerStation.Instance.RegisterPoweredLight(this);
		}
	}
}
