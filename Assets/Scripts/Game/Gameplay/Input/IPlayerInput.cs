using System;

namespace Game.Gameplay
{
    public interface IPlayerInput
    {
        event Action<float> OnHorizontalAxis;
        event Action<float> OnVerticalAxis;
    }
}
