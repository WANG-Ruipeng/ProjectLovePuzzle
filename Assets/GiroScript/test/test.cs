using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public PuzzlePieceManager manager;
    public PuzzlePiece left, right;

    private void Start()
    {
        //manager.LoadPuzzle(left, right);

    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.B))
        {
            manager.left.ChangeState();
            Debug.Log("left puzzle state: " + manager.left.state + ",right edge: " + manager.left.edgeProps[(manager.left.state + 1) % 4]);
            Debug.Log(manager.Check());
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            manager.right.ChangeState();
            Debug.Log("right puzzle state: " + manager.right.state + ",left edge: " + manager.right.edgeProps[(manager.right.state + 5) % 4]);
            Debug.Log(manager.Check());
        }
        */
    }
}
