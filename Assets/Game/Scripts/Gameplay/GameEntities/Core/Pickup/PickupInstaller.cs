using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class PickupInstaller : MonoEntityInstaller<IGameEntity>
    {
        [SerializeField] private TriggerEvents _triggerEvents;

        public override void Install(IGameEntity entity)
        {
            var condition = new AndExpression<IGameEntity>();
            condition.Add(target => target.HasPlayerTag());
            condition.Add(target => target.IsAlive());

            entity.AddIsCollected(new Variable<bool>());
            entity.AddPickupCondition(condition);
            entity.AddPickupEvent(new Atomic.Elements.Event());
            entity.AddBehaviour(new PickupBehaviour(_triggerEvents));
        }
    }
}
