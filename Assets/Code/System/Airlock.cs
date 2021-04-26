// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class Airlock : MonoBehaviour 
	{
		public DoorStation _outerDoor;
		public HeroTrigger _outerDoorAutoTrigger;
		public DoorStation _innerDoor;
		public GameObject[] _innerObjects;
		public GameObject[] _outerObjects;

		private float _countdownToOpenInnerDoor = 2f;
		private bool _initialized;

		private void HandleAutoTrigger (HeroTriggerType triggerType)
		{
			_outerDoorAutoTrigger.OnAutoTrigger -= HandleAutoTrigger;
			StartCoroutine(OpenInnerDoorAfterDelay());
		}

		private IEnumerator OpenInnerDoorAfterDelay ()
		{
			while (_outerDoor.IsClosed == false)
			{
				yield return null;
			}
			foreach (GameObject obj in _outerObjects)
			{
				obj.SetActive(false);
			}
			yield return new WaitForSeconds(_countdownToOpenInnerDoor);
			foreach (GameObject obj in _innerObjects)
			{
				obj.SetActive(true);
			}
			_innerDoor.Open();
		}

		protected void Awake ()
		{
			_outerDoorAutoTrigger.OnAutoTrigger += HandleAutoTrigger;
		}

		protected void Update ()
		{
			if (_initialized == false)
			{
				foreach (GameObject obj in _innerObjects)
				{
					obj.SetActive(false);
				}
				_initialized = true;
			}
		}
	}
}
