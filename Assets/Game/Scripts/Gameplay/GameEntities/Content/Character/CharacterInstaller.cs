using Atomic.Entities;

namespace Game.Gameplay
{
    public sealed class CharacterInstaller : MonoEntityInstaller<IGameEntity>
    {
        public override void Install(IGameEntity entity)
        {
            entity.AddPlayerTag();
        }
    }
}
