using System;

namespace Game.GameContext
{
    public interface IPlayerInput
    {
        event Action<float> OnHorizontalAxis;
        event Action<float> OnVerticalAxis;
    }
}
