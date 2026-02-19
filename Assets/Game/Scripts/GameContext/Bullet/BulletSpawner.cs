using Game.GameObjects;
using UnityEngine;

namespace Game.GameContext
{
    public sealed class BulletSpawner : MonoBehaviour
    {
        [SerializeField] private BulletPool _bulletPool;
        
        public void Spawn(Vector2 position, Vector2 direction, BulletConfig config)
        {
            var rotation = Quaternion.LookRotation(direction, Vector3.forward);
            var bullet = _bulletPool.Spawn(position, rotation);

            var movement = bullet.GetComponent<MovementComponent>();
            movement.SetDirection(direction);
            movement.SetSpeed(config.Speed);

            var bulletView = bullet.GetComponent<BulletView>();
            bulletView.SetVfx(config.IsRedVfx);
            bulletView.SetExplosionPrefab(config.ExplosionPrefab);

            bullet.gameObject.layer = config.GetLayer();

            bullet.SetDamage(config.Damage);
        }
    }
}
