using Atomic.Elements;
using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Game.UI
{
    public sealed class GameUIInstaller : MonoEntityInstaller<IPlayerContext>
    {
        [SerializeField] private Joystick _moveJoystick;
        [SerializeField] private Joystick _aimJoystick;
        [SerializeField] private GameContext _gameContext;
        [SerializeField] private StatView _healthView;
        [SerializeField] private StatView _ammoView;
        [SerializeField] private KillCountView _killCountView;

        public override void Install(IPlayerContext context)
        {
            context.AddMoveInput(new InlineValue<Vector2>(() => _moveJoystick.Direction));
            context.AddAimInput(new InlineValue<Vector2>(() => _aimJoystick.Direction));
            context.AddBehaviour(new HealthStatPresenter(_healthView));
            context.AddBehaviour(new AmmoStatPresenter(_ammoView));
            context.AddBehaviour(new KillCountPresenter(_killCountView, _gameContext));
        }
    }
}
