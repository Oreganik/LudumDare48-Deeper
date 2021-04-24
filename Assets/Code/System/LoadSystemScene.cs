// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype
{
	public class LoadSystemScene : MonoBehaviour 
	{
		protected void Awake ()
		{
			SceneManager.LoadScene("System", LoadSceneMode.Additive);
			Session.HandleSceneLoaded(SceneManager.GetActiveScene().name);
		}
	}
}
