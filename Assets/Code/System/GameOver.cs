// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype
{
	public class GameOver : MonoBehaviour 
	{
		public void ClickConfirmGameOver ()
		{
			SceneManager.LoadScene("Title", LoadSceneMode.Single);
		}

		public void ClickVote ()
		{

		}

		protected void Awake ()
		{
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;
		}
	}
}
