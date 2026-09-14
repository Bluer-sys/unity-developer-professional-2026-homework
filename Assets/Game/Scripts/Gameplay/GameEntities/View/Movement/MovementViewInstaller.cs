using Atomic.Entities;

namespace Game.Gameplay
{
    public sealed class MovementViewInstaller : MonoEntityInstaller<IGameEntity>
    {
        public override void Install(IGameEntity entity)
        {
            entity.AddBehaviour(new MovementAnimationBehaviour());
        }
    }
}
