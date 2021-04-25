// LUDUM DARE 48: DEEPER AND DEEPER
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
		public Action<HeroTriggerType> OnAutoTrigger;
		public Action<HeroTriggerType> OnHeroEnterTrigger;
		public Action<HeroTriggerType> OnHeroExitTrigger;

		public Renderer _renderer;
		public HeroTriggerType _triggerType;
		public bool _ignoreHeroRotation;
		public bool _autoTrigger;
		public GameObject _lookTargetOverride;
		public bool _ignoreLookPitch;

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

			if (OnHeroEnterTrigger != null)
			{
				OnHeroEnterTrigger(_triggerType);
			}

			if (_autoTrigger)
			{
				if (OnAutoTrigger != null)
				{
					OnAutoTrigger(_triggerType);
				}
			}
			else
			{
				_isActive = true;
			}
		}

		protected void OnTriggerExit (Collider collider)
		{
			Hero hero = collider.GetComponentInParent<Hero>();
			if (hero == null) return;
			Debug.Log("Hero exited " + _triggerType);
			_isActive = false;
			if (OnHeroExitTrigger != null)
			{
				OnHeroExitTrigger(_triggerType);
			}
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
				if (_ignoreHeroRotation == false)
				{
					// Hero must be facing parent object to trigger interaction
					Vector3 lookTarget = _lookTargetOverride != null ? _lookTargetOverride.transform.position : transform.parent.position;

					if (_ignoreLookPitch) lookTarget.y = Hero.Instance.transform.position.y;
					
					Vector3 directionHeroToGadget = (lookTarget - Hero.Instance.transform.position).normalized;
					float dot = Vector3.Dot(Hero.Instance.transform.forward, directionHeroToGadget);
					if (dot < 0.6f)
					{
						Debug.Log("Rejected: Dot is " + dot.ToString("F2"));
						return;
					}
				}
				
				if (OnStartInteract != null)
				{
					Debug.Log("Start Interact", gameObject);
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
