﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCasual.Core;
namespace Giro
{
	/// <summary>
	/// A class used to hold a reference to the current
	/// level and provide access to other classes.
	/// </summary>
	[ExecuteInEditMode]
	public class LevelManager : MonoBehaviour
	{
		/// <summary>
		/// Returns the LevelManager.
		/// </summary>
		public static LevelManager Instance => s_Instance;

		static LevelManager s_Instance;


		/// <summary>
		/// Returns the LevelDefinition used to create this LevelManager.
		/// </summary>
		public LevelDefinition LevelDefinition
		{
			get => m_LevelDefinition;
			set
			{
				m_LevelDefinition = value;

				//if (m_LevelDefinition != null && PlayerController.Instance != null)
				//{
				//    PlayerController.Instance.SetMaxXPosition(m_LevelDefinition.LevelWidth);
				//}
			}
		}
		LevelDefinition m_LevelDefinition;

		List<Movable> puzzlePieceInScene;

		/// <summary>
		/// Call this method to add a Spawnable to the list of active Spawnables.
		/// </summary>
		public void AddStep(Movable movable)
		{
			puzzlePieceInScene.Add(movable);
		}

		/// <summary>
		/// Calling this method calls the Reset() method on all Spawnables in this level.
		/// </summary>
		public void ResetLevel()
		{
			for (int i = 0; i < puzzlePieceInScene.Count; i++)
			{
				if (puzzlePieceInScene[i].isActiveAndEnabled)
				{
					if (puzzlePieceInScene[i] is PuzzlePiecePair)
					{
						PuzzlePiecePair pair = puzzlePieceInScene[i] as PuzzlePiecePair;
						pair.leftObj.GetComponentInChildren<PuzzlePiece>().Reset();
						pair.rightObj.GetComponentInChildren<PuzzlePiece>().Reset();
						//puzzlePieceInScene[i].gameObject.SetActive(false);
					}
				}
				puzzlePieceInScene[i].Reset();
			}
		}

		void Awake()
		{
			SetupInstance();
		}

		void OnEnable()
		{
			SetupInstance();

		}


		void SetupInstance()
		{
			if (s_Instance != null && s_Instance != this)
			{
				if (Application.isPlaying)
				{
					Destroy(gameObject);
				}
				else
				{
					DestroyImmediate(gameObject);
				}
				return;
			}
			if (puzzlePieceInScene == null)
				puzzlePieceInScene = new List<Movable>();
			s_Instance = this;
		}


	}
}