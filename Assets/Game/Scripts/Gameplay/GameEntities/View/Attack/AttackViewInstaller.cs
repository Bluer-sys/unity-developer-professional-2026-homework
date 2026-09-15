using Atomic.Entities;

namespace Game.Gameplay
{
    public sealed class AttackViewInstaller : MonoEntityInstaller<IGameEntity>
    {
        public override void Install(IGameEntity entity)
        {
            entity.AddBehaviour(new AttackAnimationBehaviour());
        }
    }
}
