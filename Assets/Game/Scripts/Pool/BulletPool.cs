using Game.Bullet;
using Game.Factory;

namespace Game.Pool
{
    public class BulletPool : ObjectPool<BulletBuilder, BulletFactory>
    {
        protected override void Reinitialize(BulletBuilder builder)
        {
            builder.SetToDefault();
            builder.OnDead(Despawn);
        }
    }
}
