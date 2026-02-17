using Game.GameContext.Core;
using Game.GameObjects.Bullet;

namespace Game.GameContext.Bullet
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
