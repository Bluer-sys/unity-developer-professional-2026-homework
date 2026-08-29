using Fusion;
using Game.Core;
using UnityEngine;

namespace Game.View.Core
{
    public class FireAnimComponent : NetworkBehaviour
    {
        private static readonly int Fire = Animator.StringToHash(nameof(Fire));

        [SerializeField] private FireTargetComponent _fireTargetComponent;
        [SerializeField] private Animator _animator;

        public override void Spawned()
        {
            _fireTargetComponent.OnFireStarted += OnFireStarted;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _fireTargetComponent.OnFireStarted -= OnFireStarted;
        }

        private void OnFireStarted()
        {
            _animator.SetTrigger(Fire);
        }
    }
}
