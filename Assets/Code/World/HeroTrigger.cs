﻿// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class HeroTrigger : MonoBehaviour 
	{
		public Action<HeroTriggerType> OnStartInteract;
		public Action<HeroTriggerType> OnStopInteract;

		public Renderer _renderer;
		public HeroTriggerType _triggerType;

		private bool _isActive;

		protected void Awake ()
		{
			_renderer.enabled = false;
			if (_triggerType == HeroTriggerType.Invalid)
			{
				Debug.LogWarning("Invalid trigger type on " + gameObject.name);
			}
		}

		protected void OnTriggerEnter (Collider collider)
		{
			Hero hero = collider.GetComponentInParent<Hero>();
			if (hero == null) return;
			Debug.Log("Hero entered " + _triggerType);
			switch (_triggerType)
			{
				case HeroTriggerType.LevelEnd:
					Session.LoadNextLevel();
					break;
			}
			_isActive = true;
		}

		protected void OnTriggerExit (Collider collider)
		{
			Hero hero = collider.GetComponentInParent<Hero>();
			if (hero == null) return;
			Debug.Log("Hero exited " + _triggerType);
			_isActive = false;
		}

		protected void Update ()
		{
			if (_isActive == false ||
				Hero.Instance.State == HeroState.Dead) 
			{
				return;
			}

			if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
			{
				// Hero must be facing parent object to trigger interaction
				Vector3 directionHeroToGadget = (transform.parent.position - Hero.Instance.transform.position).normalized;
				float dot = Vector3.Dot(Hero.Instance.transform.forward, directionHeroToGadget);
				if (dot < 0.6f)
				{
					Debug.Log("Rejected: Dot is " + dot.ToString("F2"));
					return;
				}
				
				if (OnStartInteract != null)
				{
					OnStartInteract(_triggerType);
				}
			}
			else if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.E))
			{
				if (OnStopInteract != null)
				{
					OnStopInteract(_triggerType);
				}
			}
		}
	}
}