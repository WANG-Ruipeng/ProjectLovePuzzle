﻿using HyperCasual.Core;
using UnityEngine;

namespace Giro
{
    [CreateAssetMenu(fileName = nameof(NormalEvent),
        menuName = "Giro/" + nameof(NormalEvent))]
    public class NormalEvent : AbstractGameEvent
    {
        public override void Reset()
        {
        }
    }
}