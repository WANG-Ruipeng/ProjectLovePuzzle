using HyperCasual.Core;
using UnityEngine;

namespace Giro
{
    /// <summary>
    /// A simple inventory class that listens to game events and keeps track of the amount of in-game currencies
    /// collected by the player
    /// </summary>
    public class Inventory : AbstractSingleton<Inventory>
    {
        [SerializeField]
        GenericGameEventListener m_WinEventListener;
        [SerializeField]
        GenericGameEventListener m_LoseEventListener;

        /// <summary>
        /// Temporary const
        /// Users keep accumulating XP when playing the game and they're rewarded as they hit a milestone.
        /// Milestones are simply a threshold to reward users for playing the game. We need to come up with
        /// a proper formula to calculate milestone values but because we don't have a plan for the milestone
        /// rewards yet, we have simple set the value to something users can never reach. 
        /// </summary>
        const float k_MilestoneFactor = 1.2f;

        TemplateUI m_Hud;
        TemplateUI m_LevelCompleteScreen;

        void Start()
        {
            m_WinEventListener.EventHandler = OnWin;
            m_LoseEventListener.EventHandler = OnLose;


            m_LevelCompleteScreen = UIManager.Instance.GetView<TemplateUI>();
            m_Hud = UIManager.Instance.GetView<TemplateUI>();
        }

        void OnEnable()
        {
            m_WinEventListener.Subscribe();
            m_LoseEventListener.Subscribe();
        }

        void OnDisable()
        {
            m_WinEventListener.Unsubscribe();
            m_LoseEventListener.Unsubscribe();
        }


        void OnWin()
        {

        }

        void OnLose()
        {
        }

        void Update()
        {
            if (m_Hud.gameObject.activeSelf)
            {

                if (SequenceManager.Instance.m_CurrentLevel is LoadLevelFromDef loadLevelFromDef)
                {
                    //在关卡开始之前的行为
                }
            }
        }
    }
}
