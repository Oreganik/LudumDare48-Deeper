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

		static MaterialPropertyBlock s_propertyBlock;

		private DoorStation _door;
		private bool _hasProblem;
		private Renderer _renderer;
		private float _timer;

		protected void Awake ()
		{
			if (s_propertyBlock == null)
			{
				s_propertyBlock = new MaterialPropertyBlock();
			}
			_door = GetComponentInParent<DoorStation>();
			_renderer = GetComponent<Renderer>();
			SetProblemActive(false, forceOperation: true);
		}

		private void SetProblemActive (bool hasProblem, bool forceOperation = false)
		{
			if (hasProblem == _hasProblem && !forceOperation) return;
			_hasProblem = hasProblem;
			_renderer.GetPropertyBlock(s_propertyBlock);
			if (hasProblem)
			{
				s_propertyBlock.SetColor("_Color", Color.white);
			}
			else
			{
				s_propertyBlock.SetColor("_Color", Color.gray);
			}
			_renderer.SetPropertyBlock(s_propertyBlock);
		}

		protected void Update ()
		{
			switch (_type)
			{
				case IndicatorType.Fuse:
					SetProblemActive(!_door.HasFuse);
					break;

				case IndicatorType.Motor:
					SetProblemActive(!_door.HasMotor);
					break;

				case IndicatorType.Power:
					SetProblemActive(!_door.HasPower);
					break;
			}
		}
	}
}
