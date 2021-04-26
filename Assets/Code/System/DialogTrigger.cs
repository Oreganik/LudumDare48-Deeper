// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class DialogTrigger : MonoBehaviour 
	{
		public Level01.DialogName _level1Dialog;

		public string[] _lines;

		public Level01 _level1;
		public GameObject _lookTarget;
		public float _zoomFov;
		public float _zoomDuration = 1;

		private HeroTrigger _heroTrigger;

		private void HandleAutoTrigger (HeroTriggerType triggerType)
		{
			_heroTrigger.OnAutoTrigger -= HandleAutoTrigger;

			if (_level1)
			{
				Debug.Log("Trigger level 1 dialog " + _level1Dialog);
				_level1.StartDialog(_level1Dialog);
			}
			else
			{
				Dialog.Instance.ShowBaked(_lines);
			}

			if (_lookTarget)
			{
				Hero.Instance.SetDialogLookTarget(_lookTarget.transform.position);
			}

			if (_zoomFov > 0)
			{
				HeroCamera.Instance.Zoom(_zoomFov, _zoomDuration);
			}
		}

		protected void Awake ()
		{
			_heroTrigger = GetComponent<HeroTrigger>();
			_heroTrigger.OnAutoTrigger += HandleAutoTrigger;
		}
	}
}
