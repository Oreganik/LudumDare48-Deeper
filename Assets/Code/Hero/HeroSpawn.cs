// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class HeroSpawn : MonoBehaviour 
	{
		public GameObject _heroPrefab;

		protected void Awake ()
		{
			if (_heroPrefab != null)
			{
				Instantiate(_heroPrefab, transform.position, transform.rotation);
			}
			gameObject.SetActive(false);
		}
	}
}
