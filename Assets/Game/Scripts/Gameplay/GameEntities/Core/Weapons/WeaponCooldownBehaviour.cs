using Atomic.Elements;
using Atomic.Entities;

namespace Game.Gameplay
{
    public sealed class WeaponCooldownBehaviour : IEntityInit<IGameEntity>, IEntityFixedTick<IGameEntity>
    {
        private ICooldown _cooldown;

        public void Init(IGameEntity entity)
        {
            _cooldown = entity.GetFireCooldown();
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            _cooldown.Tick(deltaTime);
        }
    }
}
