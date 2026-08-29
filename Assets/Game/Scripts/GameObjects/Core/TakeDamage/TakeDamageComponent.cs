using System;
using Fusion;
using Game;
using Game.Core;
using UnityEngine;

namespace SampleGame
{
    public sealed class TakeDamageComponent : NetworkBehaviour
    {
        public event Action OnDamageTaken;

        [SerializeField] 
        private HealthComponent _healthComponent;
        
        [Networked] 
        private ushort DamageCount { get; set; }
        
        private ushort _localDamageCount;
        
        public void TakeDamage(TakeDamageArgs args)
        {
            if (args.Damage <= 0 || _healthComponent.IsDead) 
                return;
            
            _healthComponent.Decrement(args.Damage);
            DamageCount++;
        }

        public override void Spawned()
        {
            _localDamageCount = DamageCount;
        }

        public override void Render()
        {
            while (_localDamageCount < DamageCount)
            {
                OnDamageTaken?.Invoke();
                _localDamageCount++;
            }
        }
    }
}
