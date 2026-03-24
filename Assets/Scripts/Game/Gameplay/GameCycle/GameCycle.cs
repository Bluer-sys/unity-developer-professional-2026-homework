using System;

namespace Game.Gameplay
{
    public class GameCycle : IGameCycle
    {
        public event Action OnGameFailed;
        public event Action OnGameWin;

        public void SetFailed()
        {
            OnGameFailed?.Invoke();
        }

        public void SetVictory()
        {
            OnGameWin?.Invoke();
        }
    }
}
