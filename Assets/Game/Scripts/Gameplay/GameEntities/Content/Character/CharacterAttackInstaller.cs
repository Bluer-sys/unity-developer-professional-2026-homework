using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class CharacterAttackInstaller : MonoEntityInstaller<IGameEntity>
    {
        [SerializeField] private GameEntity _weapon;
        [SerializeField] private GameContext _gameContext;
        [SerializeField] private float _firstAttackDelay = 0.5f;

        public override void Install(IGameEntity entity)
        {
            var delay = new Cooldown(_firstAttackDelay);
            entity.AddWeapon(_weapon);
            entity.AddFirstAttackDelay(delay);
            entity.GetAttackCondition().Add(() => entity.GetIsAiming().Value);
            entity.GetAttackCondition().Add(delay.IsCompleted);
            entity.GetAttackCondition().Add(() => ProjectileWeaponUseCase.CanFire(_weapon));
            
            entity.AddAttackAction(new InlineAction(() =>
                ProjectileWeaponUseCase.TryFire(_weapon, entity, _gameContext.GetBulletPool())));
            
            entity.AddBehaviour(new CharacterAttackBehaviour());
        }
    }
}
