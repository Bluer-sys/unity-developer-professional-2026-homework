using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Character :
        IInitializable,
        IDisposable,
        ITickable,
        IFixedTickable,
        MoveTransformComponent.ICondition,
        LookComponent.ICondition
    {
        public event Action OnPushed;
        public event Action OnBlownUp;

        private readonly HealthComponent _healthComponent;
        private readonly MoveTransformComponent _moveComponent;
        private readonly LookComponent _lookComponent;
        private readonly GroundedComponent _groundedComponent;
        private readonly ForceTargetComponent _jumpComponent;
        private readonly RigidbodyComponent _rigidbodyComponent;
        private readonly ForceAbilityComponent _pushComponent;
        private readonly ForceAbilityComponent _blowUpComponent;

        public Character(
                HealthComponent healthComponent,
                MoveTransformComponent moveComponent,
                LookComponent lookComponent,
                GroundedComponent groundedComponent,
                ForceTargetComponent jumpComponent,
                RigidbodyComponent rigidbodyComponent,
                [Inject(Id = "Push")] ForceAbilityComponent pushComponent,
                [Inject(Id = "BlowUp")] ForceAbilityComponent blowUpComponent
            )
        {
            _healthComponent = healthComponent;
            _moveComponent = moveComponent;
            _lookComponent = lookComponent;
            _groundedComponent = groundedComponent;
            _jumpComponent = jumpComponent;
            _rigidbodyComponent = rigidbodyComponent;
            _pushComponent = pushComponent;
            _blowUpComponent = blowUpComponent;
        }

        void IInitializable.Initialize()
        {
            _moveComponent.SetCondition(this);
            _lookComponent.SetCondition(this);
            
            _jumpComponent.SetCondition(() => _healthComponent.IsAlive && _groundedComponent.IsGrounded);
            _pushComponent.SetCondition(() => _healthComponent.IsAlive && !_blowUpComponent.IsProcessing);
            _blowUpComponent.SetCondition(() => _healthComponent.IsAlive && !_pushComponent.IsProcessing && _groundedComponent.IsGrounded);
            
            _pushComponent.OnPerformed += OnPushed;
            _blowUpComponent.OnPerformed += OnBlownUp;
        }

        public void Dispose()
        {
            _pushComponent.OnPerformed -= OnPushed;
            _blowUpComponent.OnPerformed -= OnBlownUp;
        }
        
        void ITickable.Tick()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _jumpComponent.ApplyForce(_rigidbodyComponent.Rigidbody);

            if (Input.GetMouseButtonDown(0))
                _pushComponent.Apply();
            
            if (Input.GetMouseButtonDown(1))
                _blowUpComponent.Apply();
        }

        void IFixedTickable.FixedTick()
        {
            MoveTick(Time.deltaTime);
        }

        private void MoveTick(float deltaTime)
        {
            float horizontal = Input.GetAxis("Horizontal");
            Vector2 direction = new Vector2(horizontal, 0f);
            
            _moveComponent.Move(direction, deltaTime);

            if (direction.x != 0)
                _lookComponent.Look(direction.x);
        }

        bool MoveTransformComponent.ICondition.Evaluate() => _healthComponent.IsAlive;
        bool LookComponent.ICondition.Evaluate() => _healthComponent.IsAlive;
    }
}
