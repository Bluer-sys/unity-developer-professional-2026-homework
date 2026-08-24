using Fusion;
using Game.Core;
using UnityEngine;

namespace Game
{
    public class Hero : NetworkBehaviour, MoveComponent.ICondition
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private MoveComponent _moveComponent;
        
        public override void Spawned()
        {
            _moveComponent.SetCondition(this);
        }

        bool MoveComponent.ICondition.IsMet()
        {
            return _healthComponent.IsAlive;
        }
    }
}
