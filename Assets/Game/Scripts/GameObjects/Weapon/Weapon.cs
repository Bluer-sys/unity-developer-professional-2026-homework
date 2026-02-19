using System;
using Game.GameContext;
using Game.Utils;
using UnityEngine;

namespace Game.GameObjects
{
    public class Weapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private BulletConfig _bulletConfig;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private BulletSpawner _bulletSpawner;
        
        private float _fireCooldown;
        private UnityTimer _timer;

        public event Action OnFired;

        public void Construct(BulletSpawner bulletSpawner)
        {
            _bulletSpawner = bulletSpawner;
        }

        public void ResetCooldown(float cooldown)
        {
            _fireCooldown = cooldown;
            _timer = new UnityTimer(_fireCooldown);
        }
        
        public bool TryFire(Vector2 direction)
        {
            if (!_timer.IsExpired())
                return false;

            Fire(direction);

            _timer.Reset();
            return true;
        }

        private void Fire(Vector2 direction)
        {
            _bulletSpawner.Spawn(_firePoint.position, direction, _bulletConfig);
            
            OnFired?.Invoke();
        }
    }
}
