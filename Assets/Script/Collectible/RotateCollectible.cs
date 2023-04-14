using Giro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCollectible : Collectible
{
	public int minRotateTime;
	public override bool Check(PuzzlePiece puzzlePiece)
	{
		if (puzzlePiece.rotateTime >= minRotateTime)
		{
			return true;
		}
		return false;
	}
	public override void SetData(LevelDefinition.CollectibleInfo collectibleInfo)
	{
		base.SetData(collectibleInfo);
		minRotateTime = collectibleInfo.minRotateTime;
	}
}
