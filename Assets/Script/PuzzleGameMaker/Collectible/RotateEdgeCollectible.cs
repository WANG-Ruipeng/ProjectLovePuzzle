using Giro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEdgeCollectible : EdgeCollectible
{
	public int minRotateTime;
	public override bool Check(PuzzlePiece puzzlePiece)
	{
		if (puzzlePiece.rotateTime < minRotateTime)
			return false;
		return base.Check(puzzlePiece);
	}
	public override void SetData(LevelDefinition.CollectibleInfo collectibleInfo)
	{
		base.SetData(collectibleInfo);
		minRotateTime = collectibleInfo.minRotateTime;
	}
}
