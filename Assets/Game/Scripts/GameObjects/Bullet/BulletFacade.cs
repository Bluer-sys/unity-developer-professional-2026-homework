using System;
using UnityEngine;

namespace Game.GameObjects.Bullet
{
    public sealed class BulletFacade : MonoBehaviour
    {
        [SerializeField] private BulletLifetime _bulletLifetime;
        
        public event Action<BulletFacade> OnDead;

        private void OnEnable()
        {
            _bulletLifetime.OnDead += OnDeadHandler;
        }

        private void OnDisable()
        {
            _bulletLifetime.OnDead -= OnDeadHandler;
        }

        private void OnDeadHandler()
        {
            OnDead?.Invoke(this);
        }
    }
}
