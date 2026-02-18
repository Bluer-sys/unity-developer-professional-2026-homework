using System;
using UnityEngine;

namespace Game.GameObjects.Core
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        private int _currentHealth;

        public event Action<int> OnHealthChanged;
        public event Action OnDead;

        public int MaxHealth { get; private set; }
        public bool IsDead { get; private set; }

        public void Construct(int maxHealth)
        {
            MaxHealth = maxHealth;
            
            ResetHealth();
        }
        
        public void Decrease(int value)
        {
            if(IsDead)
                return;
            
            _currentHealth = Mathf.Clamp(_currentHealth - value, 0, MaxHealth);

            OnHealthChanged?.Invoke(_currentHealth);
            
            if(_currentHealth == 0)
            {
                IsDead = true;
                OnDead?.Invoke();
            }
        }

        public void ResetHealth()
        {
            _currentHealth = MaxHealth;
            IsDead = false;
        }

        void IDamageable.TakeDamage(int damage)
        {
            Decrease(damage);
        }
    }
}
