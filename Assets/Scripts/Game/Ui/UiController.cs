using System;
using Game.GameContext;
using SnakeGame;
using Zenject;

namespace Game.Ui
{
    public class UiController : IInitializable, IDisposable
    {
        private readonly IGameCycle _gameCycle;
        private readonly IGameUI _gameUI;

        public UiController(IGameCycle gameCycle, IGameUI gameUI)
        {
            _gameCycle = gameCycle;
            _gameUI = gameUI;
        }

        public void Initialize()
        {
            _gameCycle.OnGameFailed += OnGameFailed;
            _gameCycle.OnDifficultyChanged += OnDifficultyChanged;
            _gameCycle.OnScoreChanged += OnScoreChanged;
            _gameCycle.OnGameWin += OnGameWin;
            
            OnScoreChanged(0);
        }

        public void Dispose()
        {
            _gameCycle.OnGameFailed -= OnGameFailed;
            _gameCycle.OnDifficultyChanged -= OnDifficultyChanged;
            _gameCycle.OnScoreChanged -= OnScoreChanged;
            _gameCycle.OnGameWin -= OnGameWin;
        }

        private void OnScoreChanged(int value)
        {
            _gameUI.SetScore(value.ToString());
        }

        private void OnGameFailed()
        {
            _gameUI.GameOver(false);
        }

        private void OnGameWin()
        {
            _gameUI.GameOver(true);
        }

        private void OnDifficultyChanged(int current, int max)
        {
            _gameUI.SetDifficulty(current, max);
        }
    }
}
