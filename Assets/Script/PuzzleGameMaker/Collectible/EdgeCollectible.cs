using Giro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeCollectible : Collectible
{
	public int rotateTime;
	public override bool Check(PuzzlePiece puzzlePiece)
	{
		if (puzzlePiece.state == rotateTime)
		{
			return true;
		}
		return false;
	}
	public override void SetData(LevelDefinition.CollectibleInfo collectibleInfo)
	{
		base.SetData(collectibleInfo);
		rotateTime = collectibleInfo.rotateTime;
	}
}
