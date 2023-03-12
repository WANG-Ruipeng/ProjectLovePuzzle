using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;

namespace Giro
{
    /// <summary>
    /// A scriptable object that stores all information
    /// needed to load and set up a Runner level.
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "Giro/LevelDefinition", order = 1)]
    public class LevelDefinition : AbstractLevelData
    {
        public Background background;
        [Header("Edge Prop从上方开始顺时针记录，从0开始")]
        public PuzzleStep[] puzzleSteps;
        [System.Serializable]
        public class PuzzleStep
        {
            [Header("左")]
            public int lPos;
            public GameObject lStepPrefab;
            public PuzzlePiece.edgeProp[] lEdgeProp;
            [Header("右")]
            public int rPos;
            public GameObject rStepPrefab;
            public PuzzlePiece.edgeProp[] rEdgeProp;
        }

        /// <summary>
        /// Store all values from updatedLevel into this LevelDefinition.
        /// </summary>
        /// <param name="updatedLevel">
        /// The LevelDefinition that has been edited in the Runner Level Editor Window.
        /// </param>
        public void SaveValues(LevelDefinition updatedLevel)
        {
            try
            {
                puzzleSteps = updatedLevel.puzzleSteps;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString() + "!!   please check this level or connect to programmer");
            }
        }
    }
}