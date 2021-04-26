// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class HeroMove : MonoBehaviour 
	{
		public float _moveSpeed = 7;
		public float _runSpeed = 14;

		private Rigidbody _rigidbody;
		private Vector3 _velocity;

		public void Process (HeroState state)
		{
			if (state == HeroState.Dead) return;
			
			if (state == HeroState.Dialog || state == HeroState.Options)
			{
				_velocity = Vector3.zero;
				return;
			}

			float x = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? -1 : 0;
			x +=  (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ? 1 : 0;
			float z = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) ? 1 : 0;
			z +=  (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) ? -1 : 0;
			//float speed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? _runSpeed : _moveSpeed;
			float speed = _moveSpeed;

			_velocity = transform.TransformVector(new Vector3(x, 0, z)) * speed;
		}

		protected void Awake ()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		protected void FixedUpdate ()
		{
			if (Hero.Instance.State == HeroState.Dead) return;
			_rigidbody.velocity = _velocity;
			_rigidbody.angularVelocity = Vector3.zero;
		}
	}
}
