// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class App : MonoBehaviour 
	{
		public static void ExitGame ()
		{
			Application.OpenURL("https://ldjam.com/events/ludum-dare/48/ctrl-base-zero");
			Application.Quit();
		}
	}
}
