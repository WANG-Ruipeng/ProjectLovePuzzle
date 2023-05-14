using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCasual.Core;
using Giro;
public class InputManager : MonoBehaviour
{
	/// <summary>
	/// Returns the InputManager.
	/// </summary>
	public static InputManager Instance => s_Instance;
	static InputManager s_Instance;
	MovableManager movableManager;
	public HUD hud;

	public bool receiveInput = false;
	void Awake()
	{
		if (s_Instance != null && s_Instance != this)
		{
			Destroy(gameObject);
			//puzzlePieceManager = PuzzlePieceManager.Instance;
			return;
		}
		s_Instance = this;
		movableManager = MovableManager.Instance;

	}

	void PlayRotateEffectSound()
	{
		if (!AudioManager.Instance) return;
		int rand = Random.Range(1, 6);
		switch (rand)
		{
			case 1:
				AudioManager.Instance.PlayEffect(SoundID.puzzle_rotate1);
				break;
			case 2:
				AudioManager.Instance.PlayEffect(SoundID.puzzle_rotate2);
				break;
			case 3:
				AudioManager.Instance.PlayEffect(SoundID.puzzle_rotate3);
				break;
			case 4:
				AudioManager.Instance.PlayEffect(SoundID.puzzle_rotate4);
				break;
			case 5:
				AudioManager.Instance.PlayEffect(SoundID.puzzle_rotate5);
				break;

		}
	}

	// Update is called once per frame
	void Update()
	{
		if (!receiveInput) { return; }
		PuzzlePiecePair pair = movableManager.GetCurrentPuzzlePair();
		if (!pair) return;
		if (!pair.left.IsRotating)
		{
			if (Input.GetKeyDown(KeyCode.W) && pair.left.rotateTime >= pair.left.puzzleSprites.Length - 1)
			{
				if (pair.left.IsLocked)
				{
					pair.left.ReleaseLockStatus();
					if (AudioManager.Instance)
						AudioManager.Instance.PlayEffect(SoundID.puzzle_lock);
					if (hud != null)
						hud.LeftLocked = false;
				}
				else
				{
					pair.left.SetLockStatus();
					if (AudioManager.Instance)
						AudioManager.Instance.PlayEffect(SoundID.puzzle_unlock);
					if (hud != null)
						hud.LeftLocked = true;
				}
			}
			else if (Input.GetKeyDown(KeyCode.Q) && !pair.left.IsLocked)
			{
				PlayRotateEffectSound();
				pair.left.ChangeState();
			}

		}

		if (!pair.right.IsRotating)
		{

			if (Input.GetKeyDown(KeyCode.P) && pair.right.rotateTime >= pair.right.puzzleSprites.Length)
			{
				if (pair.right.IsLocked)
				{
					pair.right.ReleaseLockStatus();
					if (AudioManager.Instance)
						AudioManager.Instance.PlayEffect(SoundID.puzzle_lock);
					if (hud != null)
						hud.RightLocked = false;
				}
				else
				{
					pair.right.SetLockStatus();
					if (AudioManager.Instance)
						AudioManager.Instance.PlayEffect(SoundID.puzzle_unlock);
					if (hud != null)
						hud.RightLocked = true;
				}
			}
			else if (Input.GetKeyDown(KeyCode.O) && !pair.right.IsLocked)
			{
				PlayRotateEffectSound();
				pair.right.ChangeState();
			}
		}

		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.P))
		{
			if (movableManager.Check())
			{
				//Debug.Log("Yes!");
				//pair.StartPlayingCombineAnimation();
				pair.PlayNextAnimation();
				movableManager.Collect();
				if (hud != null)
				{
					hud.LeftLocked = false;
					hud.RightLocked = false;
				}
			}
			else if (pair.left.IsLocked
				&& pair.right.IsLocked)
			{
				//Debug.Log("NOOO!");
				pair.left.ReleaseLockStatus();
				pair.right.ReleaseLockStatus();
				if (hud != null)
				{
					hud.LeftLocked = false;
					hud.RightLocked = false;
				}
			}
		}
	}
}
