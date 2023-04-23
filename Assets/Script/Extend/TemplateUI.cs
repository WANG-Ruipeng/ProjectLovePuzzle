using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCasual.Core;
namespace Giro
{
    public class TemplateUI : View
    {
        [SerializeField]
        HyperCasualButton m_StartButton;
        [SerializeField]
        HyperCasualButton m_SettingsButton;
        [SerializeField]
        AbstractGameEvent m_StartButtonEvent;

        void OnEnable()
        {
            m_StartButton.AddListener(OnStartButtonClick);
            m_SettingsButton.AddListener(OnSettingsButtonClick);
        }

        void OnDisable()
        {
            m_StartButton.RemoveListener(OnStartButtonClick);
            m_SettingsButton.RemoveListener(OnSettingsButtonClick);
        }

        void OnStartButtonClick()
        {
            m_StartButtonEvent.Raise();
        }

        void OnSettingsButtonClick()
        {
            //UIManager.Instance.Show<SettingsMenu>();
        }


    }
}