using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class EnemyInstaller : MonoEntityInstaller<IGameEntity>
    {
        [SerializeField] private GameEntity _weapon;
        [SerializeField] private GameContext _gameContext;

        public override void Install(IGameEntity entity)
        {
            entity.AddWeapon(_weapon);
            entity.AddTarget(new Variable<IGameEntity>());
            entity.AddTargetPredicate(new InlinePredicate<IGameEntity>(target =>
                target.HasPlayerTag() && target.IsAlive()));
            entity.GetAttackCondition().Add(() => TargetUseCase.HasTarget(entity));
            entity.GetAttackCondition().Add(() => MeleeWeaponUseCase.CanAttack(_weapon));
            entity.AddAttackAction(new InlineAction(() => MeleeWeaponUseCase.TryHit(_weapon, entity)));
            entity.AddBehaviour(new EnemyBehaviour());
            entity.AddBehaviour(new EnemyKillCountBehaviour(_gameContext));
        }
    }
}
