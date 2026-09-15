using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class EnemyZoneInstaller : MonoEntityInstaller<IGameEntity>
    {
        [SerializeField] private TriggerEvents _triggerEvents;
        [SerializeField] private GameEntity[] _enemies;

        public override void Install(IGameEntity entity)
        {
            entity.AddTarget(new Variable<IGameEntity>());
            entity.AddBehaviour(new EnemyZoneBehaviour(_triggerEvents, _enemies));
        }
    }
}
