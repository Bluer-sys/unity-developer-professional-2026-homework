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

        public override void Install(IPlayerContext context)
        {
            context.AddMoveInput(new InlineValue<Vector2>(() => _moveJoystick.Direction));
            context.AddAimInput(new InlineValue<Vector2>(() => _aimJoystick.Direction));
        }
    }
}
