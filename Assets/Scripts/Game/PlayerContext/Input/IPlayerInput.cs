using System;

namespace Game.PlayerContext.Input
{
    public interface IPlayerInput
    {
        event Action<float> OnHorizontalAxis;
        event Action<float> OnVerticalAxis;
    }
}
