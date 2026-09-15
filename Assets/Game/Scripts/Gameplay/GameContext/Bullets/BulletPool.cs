using Atomic.Entities;

namespace Game.Gameplay
{
    public sealed class BulletPool : MonoEntityPool<IGameEntity, GameEntity>
    {
        protected override void OnCreate(GameEntity entity)
        {
            entity.AddBulletPool(this);
            entity.gameObject.SetActive(true);
            
            base.OnCreate(entity);
        }

        protected override void OnRent(GameEntity entity)
        {
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
