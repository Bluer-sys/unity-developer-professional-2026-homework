using System;

namespace Game.Gameplay
{
    public interface IGameCycle
    {
        event Action OnGameFailed;
        event Action OnGameWin;

        void SetFailed();
        void SetVictory();
    }
}
