// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class Level02 : MonoBehaviour 
	{
		public Elevator _startingElevator;
		public GameObject _roofContainer;
		public GameObject _torch;
		public HeroTrigger _torchTrigger;

		bool _initialized = false;

		private IEnumerator OpenStartingElevator ()
		{
			yield return new WaitForSeconds(1);
			_startingElevator.Open();
		}

		private void HandleAutoTriggerTorch (HeroTriggerType type)
		{
			_torchTrigger.OnAutoTrigger -= HandleAutoTriggerTorch;
			Dialog.Instance.ShowDynamic("Glad I brought a flashlight.", "But at this point, I would have preferred a gun.");
			_torch.gameObject.SetActive(true);
			_torch.transform.parent = Hero.Instance.transform;
			_torch.transform.localPosition = (Vector3.up * 1.5f) + (_torch.transform.forward * 0.3f);
		}

		protected void Awake ()
		{
			_roofContainer.SetActive(true);
			_torchTrigger.OnAutoTrigger += HandleAutoTriggerTorch;
			_torch.gameObject.SetActive(false);
		}

		protected void Update ()
		{
			if (_initialized == false)
			{
				StartCoroutine(OpenStartingElevator());
				_initialized = true;
			}
		}
	}
}
