using UnityEngine;

namespace Game
{
    public class PushAbilityComponent : ForceAbilityComponent
    {
        public PushAbilityComponent(Settings settings, ICoroutineRunner coroutineRunner, Rigidbody2D selfRigidbody) : base(settings, coroutineRunner, selfRigidbody) {}
    }
}
