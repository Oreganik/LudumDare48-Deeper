// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class DoorIndicator : MonoBehaviour 
	{
		public enum IndicatorType { Fuse, Power, Motor }

		public IndicatorType _type;

		public Material _fuseOn;
		public Material _fuseOff;
		public Material _powerOn;
		public Material _powerOff;
		public Material _motorOn;
		public Material _motorOff;
		public Material _enabled;
		public Material _disabled;

		static MaterialPropertyBlock s_propertyBlock;

		private DoorStation _door;
		private bool _hasProblem;
		private Renderer _renderer;
		private float _timer;
		private bool _initialized;

		protected void Awake ()
		{
			if (s_propertyBlock == null)
			{
				s_propertyBlock = new MaterialPropertyBlock();
			}
			_door = GetComponentInParent<DoorStation>();
			_renderer = GetComponent<Renderer>();
			//SetProblemActive(false, forceOperation: true);
		}

		private void SetProblemActive (bool hasProblem, bool forceOperation = false)
		{
			if (hasProblem == _hasProblem && !forceOperation) return;
			_hasProblem = hasProblem;
			_renderer.sharedMaterial = hasProblem ? _disabled : _enabled;
		}

		protected void Update ()
		{
			switch (_type)
			{
				case IndicatorType.Fuse:
					SetProblemActive(!_door.HasFuse, forceOperation: !_initialized);
					break;

				case IndicatorType.Motor:
					SetProblemActive(!_door.HasMotor, forceOperation: !_initialized);
					break;

				case IndicatorType.Power:
					SetProblemActive(!_door.HasPower, forceOperation: !_initialized);
					break;
			}
			_initialized = !_initialized;
		}
	}
}
