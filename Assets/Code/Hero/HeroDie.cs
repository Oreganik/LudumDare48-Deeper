// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class HeroDie : MonoBehaviour 
	{
		const float DIE_DURATION = 3;

		public Collider _deadHeadCollider;

		private float _timer;
		private Rigidbody _rigidbody;
		private bool _hasPushed;

		public void Activate ()
		{
			enabled = true;
			_deadHeadCollider.enabled = true;
			_rigidbody.constraints = RigidbodyConstraints.None;
			_rigidbody.useGravity = true;
		}

		protected void Awake ()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_deadHeadCollider.enabled = false;
			enabled = false;
		}

		protected void FixedUpdate ()
		{
			if (_hasPushed == false)
			{
				_rigidbody.angularVelocity = UnityEngine.Random.insideUnitSphere * 90;
				_rigidbody.velocity = UnityEngine.Random.insideUnitSphere * 2;
				_hasPushed = true;
			}
		}

		protected void Update ()
		{
			_timer += Time.deltaTime;
			if (_timer > DIE_DURATION)
			{
				Session.HandleHeroDeath();
			}
		}
	}
}
