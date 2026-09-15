using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class AttackAnimationBehaviour :
        IEntityInit<IGameEntity>, 
        IEntityEnable<IGameEntity>,
        IEntityDisable<IGameEntity>
    {
        private static readonly int Attack = Animator.StringToHash("Attack");

        private Animator _animator;
        private ISignal _startedEvent;
        private ISignal _cancelledEvent;

        public void Init(IGameEntity entity)
        {
            _animator = entity.GetAnimator();
            _startedEvent = entity.GetAttackStartedEvent();
            _cancelledEvent = entity.GetAttackCancelledEvent();
        }

        public void Enable(IGameEntity entity)
        {
            _startedEvent.OnEvent += OnAttackStarted;
            _cancelledEvent.OnEvent += OnAttackCancelled;
        }

        public void Disable(IGameEntity entity)
        {
            _startedEvent.OnEvent -= OnAttackStarted;
            _cancelledEvent.OnEvent -= OnAttackCancelled;
            _animator.ResetTrigger(Attack);
        }

        private void OnAttackStarted()
        {
            _animator.SetTrigger(Attack);
        }

        private void OnAttackCancelled()
        {
            _animator.ResetTrigger(Attack);
        }
    }
}
