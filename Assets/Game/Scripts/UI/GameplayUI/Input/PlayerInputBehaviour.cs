using Atomic.Elements;
using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Game.UI
{
    public sealed class PlayerInputBehaviour :
        IEntityInit<IGameUIContext>,
        IEntityEnable<IGameUIContext>,
        IEntityDisable<IGameUIContext>,
        IEntityTick<IGameUIContext>
    {
        private readonly IGameContext _gameContext;
        private IValue<Vector2> _moveInput;
        private IValue<Vector2> _aimInput;
        private IValue<int> _health;
        private ISignal<DamageArgs> _deathEvent;
        private IVariable<Vector3> _movementDirection;
        private IVariable<Vector3> _rotationDirection;
        private IVariable<bool> _isAiming;

        public PlayerInputBehaviour(IGameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public void Init(IGameUIContext context)
        {
            IGameEntity character = _gameContext.GetCharacter();
            _moveInput = context.GetMoveInput();
            _aimInput = context.GetAimInput();
            _health = character.GetHealth();
            _deathEvent = character.GetDeathEvent();
            _movementDirection = character.GetMovementDirection();
            _rotationDirection = character.GetRotationDirection();
            _isAiming = character.GetIsAiming();
        }

        public void Enable(IGameUIContext context)
        {
            _deathEvent.OnEvent += OnDeath;
        }

        public void Disable(IGameUIContext context)
        {
            _deathEvent.OnEvent -= OnDeath;
            ResetInput();
        }

        public void Tick(IGameUIContext context, float deltaTime)
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
