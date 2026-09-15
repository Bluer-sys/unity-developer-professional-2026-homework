using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class HealthAnimationBehaviour : 
        IEntityInit<IGameEntity>,
        IEntityEnable<IGameEntity>,
        IEntityDisable<IGameEntity>
    {
        private static readonly int TakeDamage = Animator.StringToHash("TakeDamage");
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

        private IValue<int> _health;
        private ISignal<DamageArgs> _takeDamageEvent;
        private ISignal<DamageArgs> _deathEvent;
        private Animator _animator;

        public void Init(IGameEntity entity)
        {
            _health = entity.GetHealth();
            _takeDamageEvent = entity.GetTakeDamageEvent();
            _deathEvent = entity.GetDeathEvent();
            _animator = entity.GetAnimator();
        }

        public void Enable(IGameEntity entity)
        {
            _takeDamageEvent.OnEvent += OnTakeDamage;
            _deathEvent.OnEvent += OnDeath;

            if (_health.Value == 0)
                ShowDeath();
        }

        public void Disable(IGameEntity entity)
        {
            _takeDamageEvent.OnEvent -= OnTakeDamage;
            _deathEvent.OnEvent -= OnDeath;
        }

        private void OnTakeDamage(DamageArgs damage)
        {
            if (_health.Value > 0)
                _animator.SetTrigger(TakeDamage);
        }

        private void OnDeath(DamageArgs damage)
        {
            ShowDeath();
        }

        private void ShowDeath()
        {
            _animator.ResetTrigger(Attack);
            _animator.ResetTrigger(TakeDamage);
            _animator.SetBool(IsMoving, false);
            _animator.SetTrigger(Death);
        }
    }
}
