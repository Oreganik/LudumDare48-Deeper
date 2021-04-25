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
		enum State { Idle, Turn, Reset }

		public const float CRANK_DURATION = 0.5f;
		public const float RESET_DURATION = 0.5f;
		public const float TURNS_REQUIRED = 3;
		public const float WRENCH_ROTATION = -80;

		public bool IsReady
		{
			get { return _turns >= TURNS_REQUIRED; }
		}
		
		public float PercentComplete { get; private set; }

		public AudioSource[] _crankAudio;
		public AudioSource[] _resetAudio;

		public Transform _gearAxis;
		public Transform _actualGearAxis;
		public Transform _wrench;
		public Transform _wrenchAxis;
		public PoweredLight _poweredLight;
		public AudioSource _activateSound;

		private float _timer;
		private float _turns;
		private FuseStation _fuseStation;
		private HeroTrigger _trigger;
		private State _state;
		private bool _showInstructions;

		public void Crank ()
		{
			Debug.Log("Crank");
			_timer = 0;
			_state = State.Turn;
			_crankAudio[(int)_turns].Play();
		}

		public void ResetToZero (bool immediately = false)
		{
			_turns = 0;
			PercentComplete = 0;
			_actualGearAxis.localRotation = Quaternion.Euler(0, 180, PercentComplete * 180);
			_state = State.Idle;
			_poweredLight.SetLit(false, immediately);
		}

		public void SetToComplete (bool immediately = false)
		{
			_turns = TURNS_REQUIRED;
			PercentComplete = 1;
			_actualGearAxis.localRotation = Quaternion.Euler(0, 180, PercentComplete * 180);
			_state = State.Idle;
			_poweredLight.SetLit(true, immediately);
			if (immediately == false) _activateSound.Play();
		}

		private void HandleHeroEnterTrigger (HeroTriggerType triggerType)
		{
			if (triggerType != HeroTriggerType.Gear)
			{
				Debug.LogError("Weird: " + this.GetType().ToString() + " received HeroTriggerType." + triggerType.ToString());
				return;
			}
			_showInstructions = true;
		}

		private void HandleHeroExitTrigger (HeroTriggerType triggerType)
		{
			if (triggerType != HeroTriggerType.Gear)
			{
				Debug.LogError("Weird: " + this.GetType().ToString() + " received HeroTriggerType." + triggerType.ToString());
				return;
			}
			Instructions.Instance.Hide();
			_showInstructions = false;
		}

		private void HandleStartInteract (HeroTriggerType triggerType)
		{
			if (triggerType != HeroTriggerType.Gear)
			{
				Debug.LogError("Weird: " + this.GetType().ToString() + " received HeroTriggerType." + triggerType.ToString());
				return;
			}
			if (IsReady)
			{
				Debug.Log("Already set");
				return;
			}
			if (_state == State.Turn)
			{
				Debug.Log("Turning");
				return;
			}
			if (_state == State.Reset)
			{
				Debug.Log("Resetting");
				return;
			}
			if (_fuseStation.IsFuseReady == false)
			{
				Debug.Log("Fuse is not ready");
				return;
			}
			Crank();
		}

		protected void Awake ()
		{
			_trigger = GetComponentInChildren<HeroTrigger>();
			_trigger.OnStartInteract += HandleStartInteract;
			_fuseStation = GetComponentInParent<FuseStation>();
			_actualGearAxis.position = _gearAxis.position;
			_gearAxis.parent = _actualGearAxis;
			_wrenchAxis.position = _wrench.position;
			_wrench.parent = _wrenchAxis;

			_trigger.OnHeroEnterTrigger += HandleHeroEnterTrigger;
			_trigger.OnHeroExitTrigger += HandleHeroExitTrigger;
		}

		protected void Update ()
		{
			if (_showInstructions)
			{
				if (_fuseStation.IsFuseReady == false)
				{
					Instructions.Instance.Show("Door Motor", "Disabled: Bad fuse detected");
				}
				else if (_state == State.Idle && IsReady == false)
				{
					Instructions.Instance.Show("Door Motor", "Left click or E to crank start");
				}
				else
				{
					Instructions.Instance.Hide();
				}
			}

			if (_state == State.Turn)
			{
				_timer += Time.deltaTime;
				float t = Mathf.Clamp01(_timer / CRANK_DURATION);
				PercentComplete = 0;
				_wrenchAxis.localRotation = Quaternion.Euler(Vector3.forward * t * WRENCH_ROTATION);
				if (t < 1)
				{
					PercentComplete += t * (1 / TURNS_REQUIRED);
				}
				else
				{
					_resetAudio[(int)_turns].Play();
					_turns = Mathf.Clamp(_turns + 1, 0, TURNS_REQUIRED);
					_timer = 0;
					_state = State.Reset;
				}
				PercentComplete += _turns / TURNS_REQUIRED;
				// oog
				if (PercentComplete >= 1)
				{
					_activateSound.Play();
					_poweredLight.SetLit(true);
				}
				_actualGearAxis.localRotation = Quaternion.Euler(0, 180, PercentComplete * 180);
			}
			else if (_state == State.Reset)
			{
				_timer += Time.deltaTime;
				float t = Mathf.Clamp01(_timer / RESET_DURATION);
				_wrenchAxis.localRotation = Quaternion.Euler(Vector3.forward * (1 - t) * WRENCH_ROTATION);
				if (t >= 1)
				{
					_state = State.Idle;
				}
			}
		}
	}
}
