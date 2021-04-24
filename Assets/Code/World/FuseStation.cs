// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class FuseStation : MonoBehaviour 
	{
		public bool IsWorking
		{
			get { return IsFuseReady && _gear.IsReady; }
		}

		public bool IsFuseReady
		{
			get { return _fuse.State == FuseState.Good && _fuse.Door.IsClosed; }
		}

		public bool StartOpen;
		public FuseState StartFuseState;
		public TMPro.TMP_Text _debugText;

		private FuseGadget _fuse;
		private GearGadget _gear;

		protected void Awake ()
		{
			_fuse = GetComponentInChildren<FuseGadget>();
			_gear = GetComponentInChildren<GearGadget>();

			if (StartOpen)
			{
				_fuse.Door.Open(immediately: true);
			}
			else
			{
				_fuse.Door.Close(immediately: true);
			}

			_fuse.SetFuseState(StartFuseState);
		}

		protected void Update ()
		{
			if (Session.DebugVisible)
			{
				_debugText.enabled = true;
				_debugText.text = 
					"Open: " + _fuse.IsOpen + "\n" + 
					"Fuse: " + _fuse.State + "\n" +
					"Gear: " + _gear.PercentComplete.ToString("F2");
			}
			else
			{
				_debugText.enabled = false;
			}
		}
	}
}
