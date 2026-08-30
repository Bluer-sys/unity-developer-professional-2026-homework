using Fusion;
using Game.Core;
using UnityEngine;

namespace Game
{
    public sealed class DeathAnimComponent : NetworkBehaviour
    {
        private static readonly int IsDead = Animator.StringToHash(nameof(IsDead));

        [SerializeField] private Animator _animator;
        [SerializeField] private HealthComponent _healthComponent;

        public override void Spawned()
        {
            _healthComponent.OnDeath += OnDeath;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _healthComponent.OnDeath -= OnDeath;
        }

        private void OnDeath(NetworkObject obj)
        {
            _animator.SetTrigger(IsDead);
        }
    }
}
