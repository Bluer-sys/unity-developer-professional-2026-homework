using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class BulletUseCase
    {
        public static IGameEntity Spawn(IEntityPool<IGameEntity> pool, IGameEntity owner, Vector3 position, Quaternion rotation)
        {
            IGameEntity bullet = pool.Rent();
            Transform transform = bullet.GetTransform();
            
            transform.SetPositionAndRotation(position, rotation);
            bullet.GetOwner().Value = owner;
            bullet.GetLifetime().ResetTime();
            bullet.GetIsFlying().Value = true;
            transform.gameObject.SetActive(true);
            
            return bullet;
        }

        public static void Despawn(IGameEntity bullet)
        {
            IVariable<bool> isFlying = bullet.GetIsFlying();
            
            if (!isFlying.Value)
                return;

            isFlying.Value = false;
            bullet.GetBulletPool().Return(bullet);
        }

        public static void Hit(IGameEntity bullet, Collider collider)
        {
            if (!bullet.GetIsFlying().Value)
                return;

            GameEntity target = collider.GetComponentInParent<GameEntity>();
            DamageArgs damage = new DamageArgs(bullet.GetOwner().Value, bullet.GetDamage().Value);
            Despawn(bullet);

            if (target != null && target.HasValue(GameEntityAPI.Health))
                target.TryTakeDamage(damage);
        }
    }
}
