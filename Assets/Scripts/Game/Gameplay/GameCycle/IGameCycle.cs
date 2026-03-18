using System;

namespace Game.Gameplay
{
    public interface IGameCycle
    {
        event Action OnGameFailed;
        event Action<int, int> OnDifficultyChanged;
        event Action<int> OnScoreChanged;
        event Action OnGameWin;

        void SetFailed();
        void SetVictory();
        void SetDifficulty(int current, int max);
        void SetScore(int score);
    }
}
