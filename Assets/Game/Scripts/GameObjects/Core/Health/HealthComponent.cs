using System;
using Fusion;
using UnityEngine;

namespace Game.Core
{
    public sealed class HealthComponent : NetworkBehaviour
    {
        public event Action<int> OnHealthChanged;
        public event Action<NetworkObject> OnDeath;

        [Networked, OnChangedRender(nameof(InvokeStateChanged))]
        public int Current { get; set; }

        private static PropertyReader<int> HealthReader =
            GetPropertyReader<int>(typeof(HealthComponent), nameof(Current));
        
        [field: SerializeField] public int Max { get; set; } = 10;

        public bool IsDead => Current <= 0;

        public bool IsAlive => Current > 0;

        public bool IsNotFull => Current < Max;
   
        public float Progress => (float) Current / Max;

        public override void Spawned()
        {
            ResetHealth();
            InvokeHealthChanged();
        }

        public void Decrement(int damage)
        {
            Current = Math.Max(0, Current - damage);
        }

        public void Increment(int heal)
        {
            if (heal > 0) 
                Current = Math.Min(Max, Current + heal);
        }

        public void Die()
        {
            Current = 0;
        }

        private void ResetHealth()
        {
            Current = Max;
        }

        private void InvokeStateChanged(NetworkBehaviourBuffer previousSnapshot)
        {
            int previousHealth = HealthReader.Read(previousSnapshot);

            InvokeHealthChanged();
            
            if (previousHealth > 0 && IsDead)
                OnDeath?.Invoke(Object);
        }

        private void InvokeHealthChanged()
        {
            OnHealthChanged?.Invoke(Current);
        }
    }
}
