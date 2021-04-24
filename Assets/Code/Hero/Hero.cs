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

		private HeroLook _look;
		private HeroMove _move;
		private HeroState _state;

		public void Die ()
		{
			Debug.Log("Hero died");
			_state = HeroState.Dead;
			GetComponent<HeroDie>().Activate();
		}

		protected void Awake ()
		{
			Instance = this;
			_look = GetComponent<HeroLook>();
			_move = GetComponent<HeroMove>();
		}

		protected void Update ()
		{
			_look.Process(_state);
			_move.Process(_state);

			if (Input.GetKeyDown(KeyCode.Slash))
			{
				Session.ToggleDebugVisible();
			}
		}
	}
}
