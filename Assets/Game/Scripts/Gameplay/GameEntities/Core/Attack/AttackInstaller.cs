using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class AttackInstaller : MonoEntityInstaller<IGameEntity>
    {
        [SerializeField] private float _impactDelay = 0.02f;
        [SerializeField] private float _duration = 0.36f;

        public override void Install(IGameEntity entity)
        {
            AndExpression condition = new AndExpression();
            condition.Add(entity.IsAlive);

            entity.AddAttackRequest(new Request());
            entity.AddAttackDelay(new Cooldown(_impactDelay, 0));
            entity.AddAttackDuration(new Cooldown(_duration, 0));
            entity.AddAttackCondition(condition);
            entity.AddAttackStartedEvent(new Atomic.Elements.Event());
            entity.AddAttackCancelledEvent(new Atomic.Elements.Event());
            entity.AddBehaviour(new AttackBehaviour());
        }
    }
}
