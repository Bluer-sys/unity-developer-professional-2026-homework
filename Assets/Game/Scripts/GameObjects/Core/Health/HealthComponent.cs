using System;
using Fusion;
using UnityEngine;

namespace Game.Core
{
    public sealed class HealthComponent : NetworkBehaviour
    {
        public event Action<int> OnHealthChanged;
        public event Action<HealthComponent> OnHealthOver;

        [Networked, OnChangedRender(nameof(InvokeHealthChanged))]
        public int Current { get; set; }

        [field: SerializeField] public int Max { get; set; } = 10;

        public bool IsDead => Current <= 0;

        public bool IsAlive => Current > 0;

        public bool IsNotFull => Current < Max;
   
        public float Progress => (float) Current / Max;

        public override void Spawned()
        {
            ResetHealth();
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

        public void ResetHealth()
        {
            Current = Max;
        }

        private void InvokeHealthChanged()
        {
            OnHealthChanged?.Invoke(Current);
            
            if (IsDead)
                OnHealthOver?.Invoke(this);
        }
    }
}
