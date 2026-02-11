using Game.Pool;
using UnityEngine;

namespace Game.Bullet
{
    public sealed class BulletSpawner : MonoBehaviour
    {
        [SerializeField] private BulletPool _bulletPool;
        
        public void Spawn(Vector2 position, Vector2 direction, BulletConfig bulletConfig)
        {
            var rotation = Quaternion.LookRotation(direction, Vector3.forward);

            _bulletPool.Spawn(position, rotation)
                       .WithDirection(direction)
                       .WithSpeed(bulletConfig.Speed)
                       .WithLayer(bulletConfig.GetLayer())
                       .WithVfx(bulletConfig.IsRedVfx)
                       .WithDamage(bulletConfig.Damage)
                       .Build();
        }
    }
}
