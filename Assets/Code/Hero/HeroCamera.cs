// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class HeroCamera : MonoBehaviour 
	{
		public static HeroCamera Instance;

		private Camera _camera;
		private float _baseFov;
		private float _timer;
		private float _begin;
		private float _end;
		private float _duration;

		public void ResetZoom (float duration = 0.5f)
		{
			_timer = 0;
			_begin = _camera.fieldOfView;
			_end = _baseFov;
			_duration = duration;
			enabled = true;
		}

		public void Zoom (float fov, float duration)
		{
			_timer = 0;
			_begin = _camera.fieldOfView;
			_end = fov;
			_duration = duration;
			enabled = true;
		}

		protected void Awake ()
		{
			Instance = this;
			_camera = GetComponentInChildren<Camera>();
			_baseFov = _camera.fieldOfView;
			enabled = false;
		}

		protected void Update ()
		{
			_timer += Time.deltaTime;
			float t = Mathf.Clamp01(_timer / _duration);
			_camera.fieldOfView = Mathf.Lerp(_begin, _end, t);
			if (t >= 1)
			{
				enabled = false;
			}
		}
	}
}
