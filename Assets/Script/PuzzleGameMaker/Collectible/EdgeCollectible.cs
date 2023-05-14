using Giro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeCollectible : Collectible
{
	public int onEdge;
	public override bool Check(PuzzlePiece puzzlePiece)
	{
		if (puzzlePiece.RotateClockWise)
		{
			if ((puzzlePiece.state + 1) % 4 == onEdge)
				return true;
		}
		else
		{
			if ((puzzlePiece.state + 3) % 4 == onEdge)
				return true;
		}
		return false;
	}
	public override void SetData(LevelDefinition.CollectibleInfo collectibleInfo)
	{
		base.SetData(collectibleInfo);
		onEdge = collectibleInfo.onEdge;
	}
}
