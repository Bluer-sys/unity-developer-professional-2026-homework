using System;
using UnityEngine;

namespace Game
{
    public sealed class HealthComponent
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public float MaxHealth { get; private set; }
        }
        
        public float CurrentHealth { get; private set; }
        public bool IsAlive => CurrentHealth > 0;
        public bool IsDied => CurrentHealth <= 0;

        public event Action OnHealthDecreased;
        
        public event Action OnDied;

        public HealthComponent(Settings settings)
        {
            CurrentHealth = settings.MaxHealth;
        }

        public void TakeDamage(float damage)
        {
            if (!IsAlive || damage <= 0)
                return;

            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            OnHealthDecreased?.Invoke();

            if (CurrentHealth <= 0)
                OnDied?.Invoke();
        }

        public void SetZero() => 
            this.TakeDamage(this.CurrentHealth);
    }
}
