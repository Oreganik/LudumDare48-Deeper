// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class VisTrigger : MonoBehaviour 
	{
		public bool _startVisible;
		public GameObject _container;

		private bool _isVisible;

		protected void OnTriggerEnter (Collider collider)
		{
			if (!_isVisible && collider.GetComponentInParent<Hero>())
			{
				_container.SetActive(true);
				_isVisible = true;
			}
		}

		protected void OnTriggerExit (Collider collider)
		{
			if (_isVisible && collider.GetComponentInParent<Hero>())
			{
				_container.SetActive(false);
				_isVisible = false;
			}
		}

		protected void Awake ()
		{
			if (_startVisible == false)
			{
				_container.SetActive(false);
				_isVisible = false;
			}
			else
			{
				_isVisible = true;
			}
		}
	}
}
