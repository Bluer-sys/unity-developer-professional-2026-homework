using Atomic.Elements;
using Atomic.Entities;

namespace Game.Gameplay
{
    public sealed class CharacterAimingInstaller : MonoEntityInstaller<IGameEntity>
    {
        public override void Install(IGameEntity entity)
        {
            entity.AddIsAiming(new ReactiveVariable<bool>());
            entity.AddBehaviour(new CharacterAimingAnimationBehaviour());
        }
    }
}
