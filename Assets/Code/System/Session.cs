// LUDUM DARE 48: DEEPER AND DEEPER
// Copyright (c) 2021 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype
{
	public class Session 
	{
		public static bool DebugVisible { get; private set; }
		private static int _levelId;

		public static void HandleHeroDeath ()
		{
			Debug.Log("Hero is dead");
			SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
		}

		public static void HandleSceneLoaded (string sceneName)
		{
			string[] bits = sceneName.Split('_');
			if (bits.Length == 2)
			{
				int id = -1;
				if (int.TryParse(bits[1], out id))
				{
					if (id != _levelId)
					{
						Debug.Log("Overriding level ID to " + id);
						_levelId = id;
					}
				}
			}
		}

		public static void LoadNextLevel ()
		{
			_levelId++;
			string sceneName = "Level_" + _levelId.ToString("D2");
			bool foundScene = false;
			for (var i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
			{
				if (SceneUtility.GetScenePathByBuildIndex(i).Contains(sceneName))
				{
					foundScene = true;
					break;
				}
			}

			if (foundScene)
			{
				Debug.Log("load [" + sceneName + "]");
				SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
			}
			else
			{
				Debug.Log("load [GameOver]");
				SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
			}
		}

		public static void StartNewGame ()
		{
			_levelId = -1;
			LoadNextLevel();
		}

		public static void ToggleDebugVisible ()
		{
			DebugVisible = !DebugVisible;
		}
	}
}
