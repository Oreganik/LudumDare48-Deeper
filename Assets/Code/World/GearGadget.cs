// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class GearGadget : MonoBehaviour 
	{
		public const float CRANK_DURATION = 0.5f;
		public const float TURNS_REQUIRED = 3;

		public bool IsReady
		{
			get { return _turns >= TURNS_REQUIRED; }
		}
		
		public float PercentComplete { get; private set; }
		public bool IsTurning { get; private set; }

		public Transform _gearAxis;

		private float _timer;
		private float _turns;
		private FuseStation _fuseStation;
		private HeroTrigger _trigger;

		public void Crank ()
		{
			_timer = 0;
			IsTurning = true;
		}

		public void ResetToZero ()
		{
			_turns = 0;
			PercentComplete = 0;
			_gearAxis.localRotation = Quaternion.Euler(0, 180, PercentComplete * 180);
		}

		public void SetToComplete (bool immediately = false)
		{
			_turns = TURNS_REQUIRED;
			PercentComplete = 1;
			_gearAxis.localRotation = Quaternion.Euler(0, 180, PercentComplete * 180);
		}

		private void HandleStartInteract (HeroTriggerType triggerType)
		{
			if (triggerType != HeroTriggerType.Gear)
			{
				Debug.LogError("Weird: " + this.GetType().ToString() + " received HeroTriggerType." + triggerType.ToString());
				return;
			}
			if (IsTurning) return;
			if (_fuseStation.IsFuseReady == false) return;
			Crank();
		}

		protected void Awake ()
		{
			_trigger = GetComponentInChildren<HeroTrigger>();
			_trigger.OnStartInteract += HandleStartInteract;
			_fuseStation = GetComponentInParent<FuseStation>();
		}

		protected void Update ()
		{
			if (IsTurning)
			{
				_timer += Time.deltaTime;
				float t = Mathf.Clamp01(_timer / CRANK_DURATION);
				PercentComplete = 0;
				if (t < 1)
				{
					PercentComplete += t * (1 / TURNS_REQUIRED);
				}
				else
				{
					_turns = Mathf.Clamp(_turns + 1, 0, TURNS_REQUIRED);
					IsTurning = false;
				}
				PercentComplete += _turns / TURNS_REQUIRED;
				_gearAxis.localRotation = Quaternion.Euler(0, 180, PercentComplete * 180);
			}
		}
	}
}
