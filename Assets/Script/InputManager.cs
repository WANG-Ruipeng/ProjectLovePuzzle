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
    PuzzlePieceManager puzzlePieceManager;

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
        puzzlePieceManager = PuzzlePieceManager.Instance;
    }


    // Update is called once per frame
    void Update()
    {
        if (!receiveInput) { return; }

        if (!puzzlePieceManager.GetCurrentPuzzlePair().left.IsRotating)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                puzzlePieceManager.GetCurrentPuzzlePair().left.ChangeState();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (puzzlePieceManager.GetCurrentPuzzlePair().left.IsLocked)
                {
                    puzzlePieceManager.GetCurrentPuzzlePair().left.ReleaseLockStatus();
                    UIManager.Instance.GetView<HUD>().LeftLocked = false;
                }
                else
                {
                    puzzlePieceManager.GetCurrentPuzzlePair().left.SetLockStatus();
                    UIManager.Instance.GetView<HUD>().LeftLocked = true;
                }
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
                {
                    puzzlePieceManager.GetCurrentPuzzlePair().right.ReleaseLockStatus();
                    UIManager.Instance.GetView<HUD>().RightLocked = false;
                }
                else
                {
                    puzzlePieceManager.GetCurrentPuzzlePair().right.SetLockStatus();
                    UIManager.Instance.GetView<HUD>().RightLocked = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.P))
        {
            if (puzzlePieceManager.Check())
            {
                Debug.Log("Yes!");
                puzzlePieceManager.GetCurrentPuzzlePair().StartPlayingCombineAnimation();
                HUD hudWindow = UIManager.Instance.GetView<HUD>();
                hudWindow.LeftLocked = false;
                hudWindow.RightLocked = false;
            }
            else if (puzzlePieceManager.GetCurrentPuzzlePair().left.IsLocked
                && puzzlePieceManager.GetCurrentPuzzlePair().right.IsLocked)
            {
                Debug.Log("NOOO!");
                puzzlePieceManager.GetCurrentPuzzlePair().left.ReleaseLockStatus();
                puzzlePieceManager.GetCurrentPuzzlePair().right.ReleaseLockStatus();
            }
        }
    }
}
