// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class DoorBlocker : MonoBehaviour 
	{
		public HeroTrigger _trigger;
		public GameObject _collider;
		public DoorStation _door;

		private void HandleAutoTrigger (HeroTriggerType triggerType)
		{
			if (triggerType != HeroTriggerType.WalkThroughDoor)
			{
				Debug.LogError("Weird: " + this.GetType().ToString() + " received HeroTriggerType." + triggerType.ToString());
				return;
			}

			if (_collider.activeSelf == true)
			{
				Debug.Log("Already triggered door blocker");
				return;
			}

			Debug.Log("Close and block door");
			_collider.SetActive(true);
			_door.Close();
		}

		protected void Awake ()
		{
			_collider.SetActive(false);
			_trigger.OnAutoTrigger += HandleAutoTrigger;
		}
	}
}
