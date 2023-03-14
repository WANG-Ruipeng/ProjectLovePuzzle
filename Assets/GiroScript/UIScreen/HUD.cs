using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCasual.Core;
using TMPro;
using UnityEngine.UI;
namespace Giro
{
    public class HUD : View
    {
        [Header("UI On Scene")]
        public TextMeshProUGUI countdown;
        public Image leftIndicator;
        public Image rightIndicator;



        [Header("interaction Object（暂时没用）")]
        public HyperCasualButton pauseButton;
        public AbstractGameEvent pauseEvent;

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


    }
}