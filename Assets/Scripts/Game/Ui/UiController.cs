using System;
using Game.Gameplay;
using Modules;
using SnakeGame;
using Zenject;

namespace Game.Ui
{
    public class UiController : IInitializable, IDisposable
    {
        private readonly IGameCycle _gameCycle;
        private readonly IDifficulty _difficulty;
        private readonly IScore _score;
        private readonly IGameUI _gameUI;

        public UiController(IGameCycle gameCycle, IDifficulty difficulty, IScore score, IGameUI gameUI)
        {
            _gameCycle = gameCycle;
            _gameUI = gameUI;
            _difficulty = difficulty;
            _score = score;
        }

        public void Initialize()
        {
            _gameCycle.OnGameFailed += OnGameFailed;
            _difficulty.OnStateChanged += OnDifficultyChanged;
            _score.OnStateChanged += OnScoreChanged;
            _gameCycle.OnGameWin += OnGameWin;
            
            OnScoreChanged(0);
            OnDifficultyChanged();
        }

        public void Dispose()
        {
            _gameCycle.OnGameFailed -= OnGameFailed;
            _difficulty.OnStateChanged -= OnDifficultyChanged;
            _score.OnStateChanged -= OnScoreChanged;
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

        private void OnDifficultyChanged()
        {
            _gameUI.SetDifficulty(_difficulty.Current, _difficulty.Max);
        }
    }
}
