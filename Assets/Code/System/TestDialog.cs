// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class TestDialog : MonoBehaviour 
	{
		protected void Start ()
		{
			Dialog.Instance.ShowDynamic("The main generator must be going out.", "There should be backup power on each floor.");
		}
	}
}
