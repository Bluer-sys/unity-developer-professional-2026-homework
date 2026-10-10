using Atomic.Elements;

namespace Game.Gameplay
{
    public sealed class MeleeWeaponAction : IAction<IGameEntity>
    {
        private readonly IGameEntity _weapon;

        public MeleeWeaponAction(IGameEntity weapon)
        {
            _weapon = weapon;
        }

        public void Invoke(IGameEntity owner)
        {
            if (!TargetUseCase.TryFindClosest(_weapon.GetHitPoint().position, _weapon.GetHitRadius().Value,
                    _weapon.GetHitMask().Value, owner.GetTargetPredicate(), out IGameEntity target))
                return;

            if (!TargetUseCase.IsInRange(owner, target, _weapon.GetAttackDistance().Value))
                return;

            target.TryTakeDamage(new DamageArgs(owner, _weapon.GetDamage().Value));
        }
    }
}
