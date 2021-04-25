// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class HeroLook : MonoBehaviour 
	{
		public static float SensitivityX = 1;
		public static float SensitivityY = 1;
		public static bool InvertY = true;

		public float _rotateSpeed = 360;
		public float _pitchSpeed = 90;
		public float _minPitch = -80;
		public float _maxPitch = 80;
		public Transform _cameraTransform;

		private float _pitch;

		public void Process (HeroState state)
		{
			if (state == HeroState.Dead) return;
			
			if (state == HeroState.Dialog) 
			{
				if (Hero.Instance.HasLookTarget)
				{
					Vector3 turnDirection = (Hero.Instance.LookTarget - transform.position).normalized;
					turnDirection.y = 0;
					transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(turnDirection, Vector3.up), 0.05f);

					Vector3 lookDirection = (Hero.Instance.LookTarget - _cameraTransform.position).normalized;
					_pitch = Mathf.Lerp(_pitch, lookDirection.x, 0.05f);
					_pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);
					_cameraTransform.localRotation = Quaternion.Euler(Vector3.right * _pitch);
				}
				return;
			}

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			float inputX = Input.GetAxis("Mouse X");
			float inputY = Input.GetAxis("Mouse Y");

			if (Application.isEditor && Time.timeSinceLevelLoad < 1)
			{
				if (Mathf.Abs(inputX) > 1 || Mathf.Abs(inputY) > 1)
				{
					return;
				}
			}

			float rotate = inputX * SensitivityX * _rotateSpeed * Time.deltaTime;
			transform.Rotate(Vector3.up * rotate, Space.World);

			float pitchInput = inputY * SensitivityY * _pitchSpeed * Time.deltaTime;
			if (InvertY) pitchInput *= -1;

			_pitch = Mathf.Clamp(_pitch + pitchInput, _minPitch, _maxPitch);
			_cameraTransform.localRotation = Quaternion.Euler(Vector3.right * _pitch);
		}
	}
}
