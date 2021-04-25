// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class Hero : MonoBehaviour 
	{
		public static Hero Instance;

		public HeroState State
		{
			get { return _state; }
		}

		public Vector3 LookTarget { get; private set; } 
		public int FuseCount { get; private set; }
		public bool HasLookTarget { get; private set; }

		private HeroLook _look;
		private HeroMove _move;
		private HeroState _state;

		public void AddFuse ()
		{
			FuseCount++;
		}

		public void ClearDialogLookTarget ()
		{
			HasLookTarget = false;
		}

		public void Die ()
		{
			Debug.Log("Hero died");
			_state = HeroState.Dead;
			GetComponent<HeroDie>().Activate();
		}

		public void RemoveFuse ()
		{
			FuseCount = Mathf.Max(FuseCount - 1, 0);
		}

		public void SetDialogLookTarget (Vector3 targetPosition)
		{
			LookTarget = targetPosition;
			HasLookTarget = true;
		}

		protected void Awake ()
		{
			Instance = this;
			_look = GetComponent<HeroLook>();
			_move = GetComponent<HeroMove>();
		}

		protected void Update ()
		{
			if (Dialog.Instance && Dialog.Instance.IsActive)
			{
				_look.Process(HeroState.Dialog);
				_move.Process(HeroState.Dialog);
			}
			else
			{
				_look.Process(_state);
				_move.Process(_state);
			}

			if (Input.GetKeyDown(KeyCode.Slash))
			{
				Session.ToggleDebugVisible();
			}
		}
	}
}
