using UnityEngine;

namespace Game
{
    public class BlowUpAbilityComponent : ForceAbilityComponent
    {
        public BlowUpAbilityComponent(Settings settings, ICoroutineRunner coroutineRunner, Rigidbody2D selfRigidbody) : base(settings, coroutineRunner, selfRigidbody) {}
    }
}
