using Atomic.Elements;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class ProjectileWeaponAction : IAction<IGameEntity>
    {
        private readonly IGameEntity _weapon;
        private readonly IGameContext _gameContext;
        private readonly IAction _action;

        public ProjectileWeaponAction(IGameEntity weapon, IGameContext gameContext, IAction action)
        {
            _weapon = weapon;
            _gameContext = gameContext;
            _action = action;
        }

        public void Invoke(IGameEntity owner)
        {
            Transform firePoint = _weapon.GetFirePoint();
            float spread = _weapon.GetSpreadAngle().Value;
            float spreadX = Random.Range(-spread, spread);
            float spreadY = Random.Range(-spread, spread);
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(spreadX, spreadY, 0);

            _action.Invoke();
            BulletUseCase.Spawn(_gameContext.GetBulletPool(), owner, firePoint.position, rotation);
            _weapon.GetFireEvent().Invoke();
        }
    }
}
