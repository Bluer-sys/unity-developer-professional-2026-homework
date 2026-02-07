using System;
using UnityEngine;

namespace Game.Common
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _baseHealth;

        private int _currentHealth;
        private bool _isDead;

        public event Action<int> OnHealthChanged;
        public event Action OnDead;
        
        public int MaxHealth => _baseHealth;

        public void Awake()
        {
            _currentHealth = _baseHealth;
        }

        public void Decrease(int value)
        {
            if(_isDead)
                return;
            
            _currentHealth = Mathf.Clamp(_currentHealth - value, 0, _baseHealth);

            OnHealthChanged?.Invoke(_currentHealth);
            
            if(_currentHealth == 0)
            {
                _isDead = true;
                OnDead?.Invoke();
            }
        }

    }
}
