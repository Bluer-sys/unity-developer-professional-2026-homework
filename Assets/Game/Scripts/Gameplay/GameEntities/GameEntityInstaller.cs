using Atomic.Entities;

namespace Game.Gameplay
{
    public sealed class GameEntityInstaller : MonoEntityInstaller<IGameEntity>
    {
        public override void Install(IGameEntity entity)
        {
            entity.AddTransform(transform);
        }
    }
}
