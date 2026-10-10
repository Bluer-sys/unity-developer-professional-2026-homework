using Atomic.Elements;
using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Game.UI
{
    public sealed class GameUIInstaller : MonoEntityInstaller<IGameUIContext>
    {
        [SerializeField] private GameContext _gameContext;
        [SerializeField] private Joystick _moveJoystick;
        [SerializeField] private Joystick _aimJoystick;
        [SerializeField] private StatView _healthView;
        [SerializeField] private StatView _ammoView;
        [SerializeField] private KillCountView _killCountView;

        public override void Install(IGameUIContext context)
        {
            context.AddMoveInput(new InlineValue<Vector2>(() => _moveJoystick.Direction));
            context.AddAimInput(new InlineValue<Vector2>(() => _aimJoystick.Direction));
            context.AddBehaviour(new PlayerInputBehaviour(_gameContext));
            context.AddBehaviour(new HealthStatPresenter(_healthView, _gameContext));
            context.AddBehaviour(new AmmoStatPresenter(_ammoView, _gameContext));
            context.AddBehaviour(new KillCountPresenter(_killCountView, _gameContext));
        }
    }
}
