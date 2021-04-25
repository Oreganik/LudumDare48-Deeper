// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype
{
	public class Instructions : MonoBehaviour 
	{
		public static Instructions Instance;

		public GameObject _container;
		public Text _instructionsText;

		public void Hide ()
		{
			_container.SetActive(false);
		}

		public void Show (string title, string text)
		{
			_container.SetActive(true);
			_instructionsText.text = "<b>" + title.ToUpper() + "</b>\n" + text;
		}

		protected void Awake ()
		{
			Instance = this;
			Hide();
		}
	}
}
