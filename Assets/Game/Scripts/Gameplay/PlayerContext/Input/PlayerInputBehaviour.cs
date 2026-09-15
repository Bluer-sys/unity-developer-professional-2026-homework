using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class PlayerInputBehaviour :
        IEntityInit<IPlayerContext>, 
        IEntityEnable<IPlayerContext>,
        IEntityDisable<IPlayerContext>, 
        IEntityTick<IPlayerContext>
    {
        private IValue<Vector2> _moveInput;
        private IValue<Vector2> _aimInput;
        private IValue<int> _health;
        private ISignal<DamageArgs> _deathEvent;
        private IVariable<Vector3> _movementDirection;
        private IVariable<Vector3> _rotationDirection;
        private IVariable<bool> _isAiming;

        public void Init(IPlayerContext context)
        {
            IGameEntity character = context.GetCharacter();
            _moveInput = context.GetMoveInput();
            _aimInput = context.GetAimInput();
            _health = character.GetHealth();
            _deathEvent = character.GetDeathEvent();
            _movementDirection = character.GetMovementDirection();
            _rotationDirection = character.GetRotationDirection();
            _isAiming = character.GetIsAiming();
        }

        public void Enable(IPlayerContext context)
        {
            _deathEvent.OnEvent += OnDeath;
        }

        public void Disable(IPlayerContext context)
        {
            _deathEvent.OnEvent -= OnDeath;
            ResetInput();
        }

        public void Tick(IPlayerContext context, float deltaTime)
        {
            if (_health.Value == 0)
            {
                ResetInput();
                return;
            }

            Vector2 moveInput = _moveInput.Value;
            Vector2 aimInput = _aimInput.Value;
            Vector3 movementDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            Vector3 aimDirection = new Vector3(aimInput.x, 0, aimInput.y).normalized;

            _movementDirection.Value = movementDirection;
            _rotationDirection.Value = aimDirection != Vector3.zero ? aimDirection : movementDirection;
            _isAiming.Value = aimDirection != Vector3.zero;
        }

        private void OnDeath(DamageArgs damage)
        {
            ResetInput();
        }

        private void ResetInput()
        {
            _movementDirection.Value = Vector3.zero;
            _rotationDirection.Value = Vector3.zero;
            _isAiming.Value = false;
        }
    }
}
