// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype
{
	public class Dialog : MonoBehaviour 
	{
		public static Dialog Instance;

		public Action OnDialogClose;

		const float DELAY_BETWEEN_CHARACTERS = 0.05f;

		public Text _dialogText;
		public GameObject _dialogContainer;
		public GameObject _buttonOk;
		public bool IsActive { get; private set; }

		private List<string> _lines;
		private int _index;
		private int _char;
		private float _timer;

		public void ClickOk ()
		{
			_index++;
			if (_index >= _lines.Count)
			{
				Hide();
			}
			else
			{
				_char = 0;
				_dialogText.text = "";
				_timer = 1;
			}
		}

		public void Hide ()
		{
			enabled = false;
			_dialogContainer.SetActive(false);
			_buttonOk.SetActive(false);
			IsActive = false;
			if (OnDialogClose != null)
			{
				OnDialogClose();
			}
		}

		public void ShowBaked (string[] lines)
		{
			_lines = new List<string>(lines);
			enabled = true;
			_index = 0;
			_char = 0;
			_dialogContainer.SetActive(true);
			_dialogText.text = "";
			_timer = 1;
			IsActive = true;
		}

		public void ShowDynamic (params string[] lines)
		{
			ShowBaked(lines);
		}

		protected void Awake ()
		{
			Instance = this;
			enabled = false;
			_dialogContainer.SetActive(false);
			_buttonOk.SetActive(false);
		}

		protected void LateUpdate ()
		{
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;

			int charCount = _lines[_index].Length -1;
			if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))
			{
				_char = charCount;
			}
			else
			{
				_timer += Time.deltaTime;
				if (_timer >= DELAY_BETWEEN_CHARACTERS)
				{
					_char = Mathf.Min(_char + 1, charCount);
					_timer = 0;
				}
			}

			_dialogText.text = _lines[_index].Substring(0, _char);

			if (_char >= charCount)
			{
				_buttonOk.SetActive(true);
			}
		}
	}
}
