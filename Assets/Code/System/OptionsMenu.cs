// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype
{
	public class OptionsMenu : MonoBehaviour 
	{
		public static OptionsMenu Instance;

		public bool IsOpen
		{
			get { return gameObject.activeSelf; }
		}

		public Text _sensitivity;

		private AudioSource _clickSound;

		public void ClickContinue ()
		{
			_clickSound.Play();
			Close();
		}

		public void ClickQuit ()
		{
			App.ExitGame();
		}

		public void Close ()
		{
			gameObject.SetActive(false);
		}

		public void Open ()
		{
			gameObject.SetActive(true);
			_clickSound.Play();
		}

		protected void Awake ()
		{
			Instance = this;
			_clickSound = GetComponent<AudioSource>();
			Close();
		}

		private void SetSensitivity (float value)
		{
			_clickSound.Play();
			HeroLook.Sensitivity = value / 10;
		}

		protected void Update ()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1)) SetSensitivity(1);
			if (Input.GetKeyDown(KeyCode.Alpha2)) SetSensitivity(2);
			if (Input.GetKeyDown(KeyCode.Alpha3)) SetSensitivity(3);
			if (Input.GetKeyDown(KeyCode.Alpha4)) SetSensitivity(4);
			if (Input.GetKeyDown(KeyCode.Alpha5)) SetSensitivity(5);
			if (Input.GetKeyDown(KeyCode.Alpha6)) SetSensitivity(6);
			if (Input.GetKeyDown(KeyCode.Alpha7)) SetSensitivity(7);
			if (Input.GetKeyDown(KeyCode.Alpha8)) SetSensitivity(8);
			if (Input.GetKeyDown(KeyCode.Alpha9)) SetSensitivity(9);

			_sensitivity.text = (HeroLook.Sensitivity * 10).ToString();

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				_clickSound.Play();
				Close();
			}
		}
	}
}
