using System;

namespace Game.PlayerContext.SnakeContext
{
    public interface ISnakeHead
    {
        event Action OnCollided;
    }
}
