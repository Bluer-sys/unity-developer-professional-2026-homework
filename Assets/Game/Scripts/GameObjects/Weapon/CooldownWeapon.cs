using System;
using Game.GameContext.Bullet;
using Game.GameObjects.Bullet;
using Game.Utils;
using UnityEngine;

namespace Game.GameObjects.Weapon
{
    public class CooldownWeapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private Transform _firePoint;

        private BulletSpawner _bulletSpawner;
        private BulletConfig _bulletConfig;
        private float _fireCooldown;

        private UnityTimer _timer;

        public event Action OnFired;
        

        public void Construct(BulletSpawner bulletSpawner, 
                              BulletConfig bulletConfig, 
                              float cooldown)
        {
            _bulletSpawner = bulletSpawner;
            _bulletConfig = bulletConfig;
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
