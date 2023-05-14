using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Runtime.Signals;
using Giro;
using Doozy.Runtime.Nody;

public class MainScreen : MonoBehaviour
{
	public FlowController flow;
	public FlowNode firstEntryNode;
	public FlowNode levelSelectNode;
	public void CheckFirstEntry()
	{
		if (SaveManager.LevelProgress == 0)
		{
			SequenceManager.Instance.ShowIllustration(0);
			SequenceManager.Instance.CurrentLevel = 1;
		}
		else
		{
			flow.SetActiveNode(levelSelectNode);
		}
	}
}
