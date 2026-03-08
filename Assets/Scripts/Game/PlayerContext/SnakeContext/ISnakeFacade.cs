using System;

namespace Game.PlayerContext.SnakeContext
{
    public interface ISnakeFacade
    {
        event Action OnCollided;

        void SetActive(bool isActive);
    }
}
