using System;
using Game.Bullet;
using Game.Interfaces;
using UnityEngine;

namespace Game.Weapon
{
    public class CooldownWeapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private Transform _firePoint;

        private BulletSpawner _bulletSpawner;
        private BulletConfig _bulletConfig;
        private float _fireCooldown;

        private float _lastFireTime;

        public event Action OnFired;
        

        public void Construct(BulletSpawner bulletSpawner, 
                              BulletConfig bulletConfig, 
                              float cooldown)
        {
            _bulletSpawner = bulletSpawner;
            _bulletConfig = bulletConfig;
            _fireCooldown = cooldown;
        }
        
        public bool TryFire(Vector2 direction)
        {
            if (Time.time - _lastFireTime < _fireCooldown)
                return false;

            Fire(direction);

            _lastFireTime = Time.time;
            return true;
        }

        private void Fire(Vector2 direction)
        {
            _bulletSpawner.Spawn(_firePoint.position, direction, _bulletConfig);
            
            OnFired?.Invoke();
        }
    }
}
