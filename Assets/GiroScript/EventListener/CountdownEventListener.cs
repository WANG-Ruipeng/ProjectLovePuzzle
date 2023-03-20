using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperCasual.Core;
using UnityEngine;
namespace Giro
{
    internal class CountdownEventListener : IGameEventListener<bool>
    {
        public void OnEventRaised(bool isCountingdown)
        {
            //GameManager gm = GameManager.Instance;
            //if (!gm.isCountingDown)
            //{
            //    gm.StartTime = Time.time;
            //}
            //gm.isCountingDown = !gm.isCountingDown;
            //InputManager.Instance.receiveInput = gm.isCountingDown;
        }

    }
}
