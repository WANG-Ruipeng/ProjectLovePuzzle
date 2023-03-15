using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    /// <summary>
    /// Returns the InputManager.
    /// </summary>
    public static InputManager Instance => s_Instance;
    static InputManager s_Instance;
    PuzzlePieceManager puzzlePieceManager;
    void Awake()
    {
        if (s_Instance != null && s_Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        puzzlePieceManager = PuzzlePieceManager.Instance;
        s_Instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        if (!puzzlePieceManager.GetCurrentPuzzlePair().left.IsRotating)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                puzzlePieceManager.GetCurrentPuzzlePair().left.ChangeState();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (puzzlePieceManager.GetCurrentPuzzlePair().left.IsLocked)
                    puzzlePieceManager.GetCurrentPuzzlePair().left.ReleaseLockStatus();
                else
                    puzzlePieceManager.GetCurrentPuzzlePair().left.SetLockStatus();
            }
        }

        if (!puzzlePieceManager.GetCurrentPuzzlePair().right.IsRotating)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                puzzlePieceManager.GetCurrentPuzzlePair().right.ChangeState();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                if (puzzlePieceManager.GetCurrentPuzzlePair().right.IsLocked)
                    puzzlePieceManager.GetCurrentPuzzlePair().right.ReleaseLockStatus();
                else
                    puzzlePieceManager.GetCurrentPuzzlePair().right.SetLockStatus();
            }
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.P))
        {
            if (puzzlePieceManager.Check())
            {
                puzzlePieceManager.GetCurrentPuzzlePair().StartPlayingCombineAnimation();
            }
            else if (puzzlePieceManager.GetCurrentPuzzlePair().left.IsLocked 
                && puzzlePieceManager.GetCurrentPuzzlePair().right.IsLocked)
            {
                puzzlePieceManager.GetCurrentPuzzlePair().left.ReleaseLockStatus();
                puzzlePieceManager.GetCurrentPuzzlePair().right.ReleaseLockStatus();
            }
        }
    }
}
