using System;

namespace Game.Gameplay
{
    public class GameCycle : IGameCycle
    {
        public event Action OnGameFailed;
        public event Action<int, int> OnDifficultyChanged;
        public event Action<int> OnScoreChanged;
        public event Action OnGameWin;

        public void SetFailed()
        {
            OnGameFailed?.Invoke();
        }

        public void SetVictory()
        {
            OnGameWin?.Invoke();
        }
        
        public void SetDifficulty(int current, int max)
        {
            OnDifficultyChanged?.Invoke(current, max);
        }
        
        public void SetScore(int score)
        {
            OnScoreChanged?.Invoke(score);
        }
    }
}
