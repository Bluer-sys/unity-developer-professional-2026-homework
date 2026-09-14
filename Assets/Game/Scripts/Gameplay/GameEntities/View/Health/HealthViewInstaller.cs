using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class HealthViewInstaller : MonoEntityInstaller<IGameEntity>
    {
        [SerializeField] private Animator _animator;

        public override void Install(IGameEntity entity)
        {
            entity.AddAnimator(_animator);
            entity.AddBehaviour(new HealthAnimationBehaviour());
        }
    }
}
