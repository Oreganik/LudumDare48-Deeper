// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class Title : MonoBehaviour 
	{
		public GameObject _titleStuff;

		public void ClickStart ()
		{
			_titleStuff.SetActive(false);
			Dialog.Instance.ShowDynamic(
				"The emergency beacon at CTRL Base Zero was set off a few hours ago.",
				"Once I arrive, I need to assess the situation and brief the general.",
				"Their base command is on level -2, so that's my target."
			);
			Dialog.Instance.OnDialogClose += HandleDialogClosed;
		}

		private void HandleDialogClosed ()
		{
			Dialog.Instance.OnDialogClose -= HandleDialogClosed;
			Session.StartNewGame();
		}

		protected void Awake ()
		{
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;
		}

		protected void Update ()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				App.ExitGame();
			}
		}
	}
}
