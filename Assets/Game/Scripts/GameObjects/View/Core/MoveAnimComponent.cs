using Fusion;
using Game.Core;
using UnityEngine;

namespace Game.View.Core
{
    public class MoveAnimComponent : NetworkBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash(nameof(IsMoving));

        [SerializeField]
        private MoveComponent _moveComponent;

        [SerializeField]
        private Animator _animator;
        
        public override void Spawned()
        {
            _moveComponent.OnStateChange += OnStateChange;
            OnStateChange();
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _moveComponent.OnStateChange -= OnStateChange;
        }

        private void OnStateChange()
        {
            _animator.SetBool(IsMoving, _moveComponent.IsMoving);
        }
    }
}
