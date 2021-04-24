﻿// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class Title : MonoBehaviour 
	{
		public void ClickStart ()
		{
			Session.StartNewGame();
		}

		protected void Awake ()
		{
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;
		}
	}
}
