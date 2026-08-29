using Fusion;
using Game.Core;
using UnityEngine;

namespace Game
{
    public class Hero : NetworkBehaviour,
                        MoveComponent.ICondition,
                        FireTargetComponent.ICondition, 
                        RotateTargetComponent.ICondition
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField] private FireTargetComponent _fireTargetComponent;
        [SerializeField] private RotateTargetComponent _rotateTargetComponent;
        
        public override void Spawned()
        {
            _moveComponent.SetCondition(this);
            _fireTargetComponent.SetCondition(this);
            _rotateTargetComponent.SetCondition(this);
        }

        bool MoveComponent.ICondition.IsMet()
        {
            return _healthComponent.IsAlive;
        }

        bool FireTargetComponent.ICondition.IsMet()
        {
            return _healthComponent.IsAlive && !_moveComponent.IsMoving;
        }

        bool RotateTargetComponent.ICondition.IsMet()
        {
            return _healthComponent.IsAlive && !_moveComponent.IsMoving;
        }
    }
}
