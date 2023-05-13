using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Doozy.Runtime.UIManager.Containers;
using Doozy.Runtime.Reactor;

namespace Giro
{
	public class HUD : MonoBehaviour
	{
		[Header("UI On Scene")]
		public TextMeshProUGUI countdown;
		public Image leftIndicator;
		public Image rightIndicator;
		public Progressor progressor;


		public void UpdateValueBar(float progress, float stepNum)
		{
			float value = progress / stepNum;
			if (value < 0 || value > 1)
			{
				Debug.LogError("Invalid Progress value!!!!!!!   ----HUD");
				return;
			}
			progressor.PlayToValue(value);
		}
		public bool LeftLocked//调用UIManager.Instance.GetView<HUD>()来获取这个东西的实例
		{
			get => leftLocked;
			set
			{
				leftLocked = value;
				if (leftLocked) LockIndicator(leftIndicator);
				else UnlockIndicator(leftIndicator);
			}
		}
		bool leftLocked;
		public bool RightLocked
		{
			get => rightLocked;
			set
			{
				rightLocked = value;
				if (rightLocked) LockIndicator(rightIndicator);
				else UnlockIndicator(rightIndicator);
			}
		}

		bool rightLocked;

		int timeLeft;
		public float TimeLeft
		{
			get => timeLeft;
			set
			{
				timeLeft = (int)Mathf.Ceil(value);
				countdown.text = timeLeft.ToString();
			}
		}

		void LockIndicator(Image image)//具体表现形式等待美术提要求
		{
			Color c = image.color;
			c.a = 255;
			image.color = c;
		}

		void UnlockIndicator(Image image)
		{
			Color c = image.color;
			c.a = 0;
			image.color = c;
		}
	}
}