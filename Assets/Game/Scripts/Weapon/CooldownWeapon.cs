using System;
using Game.Bullet;
using Game.Data;
using Game.Interfaces;
using UnityEngine;

namespace Game.Weapon
{
    public class CooldownWeapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private Transform _firePoint;
        
        private TeamType _team;
        private float _fireCooldown;
        private BulletWorldGo _bulletWorld;
        private BulletConfig _bulletConfig;

        private float _lastFireTime;

        public event Action OnFired;
        

        public void Construct(TeamType team, 
                              BulletWorldGo bulletWorldGo, 
                              float cooldown, 
                              BulletConfig bulletConfig)
        {
            _team = team;
            _fireCooldown = cooldown;
            _bulletWorld = bulletWorldGo;
            _bulletConfig = bulletConfig;
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
            _bulletWorld.Spawn(_firePoint.position,
                direction,
                _bulletConfig.Speed,
                _bulletConfig.Damage,
                _team);
            
            OnFired?.Invoke();
        }
    }
}
