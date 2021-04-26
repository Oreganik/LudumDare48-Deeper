// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class Torch : MonoBehaviour 
	{
		protected void Start ()
		{
			transform.parent = Hero.Instance.transform;
			transform.localPosition = (Vector3.up * 1.5f) + (transform.forward * 0.3f);
		}
	}
}
