using Game.Pool;
using UnityEngine;

namespace Game.Bullet
{
    public sealed class BulletSpawner : MonoBehaviour
    {
        [SerializeField] private BulletPool _bulletPool;
        [SerializeField] private Transform _container;
        
        public void Spawn(Vector2 position, Vector2 direction, BulletConfig bulletConfig)
        {
            var rotation = Quaternion.LookRotation(direction, Vector3.forward);

            _bulletPool.Spawn(position, rotation, _container).SetDirection(direction)
                       .SetSpeed(bulletConfig.Speed)
                       .SetLayer(bulletConfig.GetLayer())
                       .SetVfx(bulletConfig.IsRedVfx)
                       .SetDamage(bulletConfig.Damage)
                       .OnDead(OnBulletDead);
        }

        private void OnBulletDead(BulletBuilder builder)
        {
            _bulletPool.Despawn(builder);
        }
    }
}
