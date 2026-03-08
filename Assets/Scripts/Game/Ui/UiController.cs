using System;
using Game.PlayerContext.SnakeContext;
using Game.SceneContext;
using Modules;
using SnakeGame;
using Zenject;

namespace Game.Ui
{
    public class UiController : IInitializable, IDisposable
    {
        private readonly IGameUI _gameUI;
        private readonly ISnakeFacade _snakeFacade;
        private readonly IDifficulty _difficulty;
        private readonly IScore _score;
        private readonly ICoinsSpawner _coinsSpawner;

        public UiController(
            IGameUI gameUI,
            ISnakeFacade snakeFacade,
            IDifficulty difficulty,
            IScore score,
            ICoinsSpawner coinsSpawner)
        {
            _gameUI = gameUI;
            _snakeFacade = snakeFacade;
            _difficulty = difficulty;
            _score = score;
            _coinsSpawner = coinsSpawner;
        }

        public void Initialize()
        {
            _snakeFacade.OnCollided += SetGameFailed;
            _difficulty.OnStateChanged += OnDifficultyChanged;
            _score.OnStateChanged += OnScoreChanged;
            _coinsSpawner.OnCoinsOver += SetGameWin;
            
            OnScoreChanged(0);
            OnDifficultyChanged();
        }

        public void Dispose()
        {
            _snakeFacade.OnCollided -= SetGameFailed;
            _difficulty.OnStateChanged -= OnDifficultyChanged;
            _score.OnStateChanged -= OnScoreChanged;
            _coinsSpawner.OnCoinsOver -= SetGameWin;
        }

        private void OnScoreChanged(int value)
        {
            _gameUI.SetScore(value.ToString());
        }

        private void SetGameFailed()
        {
            _gameUI.GameOver(false);
        }

        private void SetGameWin()
        {
            _gameUI.GameOver(true);
        }

        private void OnDifficultyChanged()
        {
            int current = _difficulty.Current;
            int max = _difficulty.Max;
            
            _gameUI.SetDifficulty(current, max);
        }
    }
}
