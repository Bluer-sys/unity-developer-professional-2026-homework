using Fusion;
using Game.Core;
using UnityEngine;
using Transform = log4net.Util.Transform;

namespace Game.GameObjects
{
    public class Enemy : NetworkBehaviour, MoveComponent.ICondition
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private MoveComponent _moveComponent;
        
        private Vector3 _moveDirection;

        public override void Spawned()
        {
            _moveComponent.SetCondition(this);
        }

        bool MoveComponent.ICondition.IsMet()
            => _healthComponent.IsAlive;

        public override void FixedUpdateNetwork()
        {
            _moveComponent.Move(_moveDirection);
        }

        public void SetMoveDirection(Vector3 direction)
        {
            _moveDirection = direction;
        }
    }
}
