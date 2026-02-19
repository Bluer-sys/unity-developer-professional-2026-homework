using Game.GameObjects;

namespace Game.GameContext
{
    public class BulletPool : ObjectPool<Bullet, BulletFactory>
    {
        protected override void Reinitialize(Bullet bullet)
        {
            bullet.OnDead += Despawn;
        }

        protected override void OnDespawned(Bullet bullet)
        {
            bullet.OnDead -= Despawn;
        }
    }
}
