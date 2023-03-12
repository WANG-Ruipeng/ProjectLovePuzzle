using HyperCasual.Core;
using UnityEngine;

namespace Giro
{
    /// <summary>
    /// The event is triggered when the player picks up an item (like coin and keys)
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ItemPickedEvent),
        menuName = "Giro/" + nameof(ItemPickedEvent))]
    public class ItemPickedEvent : AbstractGameEvent
    {
        [HideInInspector]
        public int Count = -1;

        public override void Reset()
        {
            Count = -1;
        }
    }
}
