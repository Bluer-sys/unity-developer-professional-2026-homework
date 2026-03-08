using System;
using Game.GameContext;
using Modules;
using Zenject;

namespace Game.PlayerContext.SnakeContext
{
    public class SnakeSpeedController : IInitializable, IDisposable
    {
        private readonly GameConfig _config;
        private readonly ISnake _snake;
        private readonly IDifficulty _difficulty;

        public SnakeSpeedController(GameConfig config, ISnake snake, IDifficulty difficulty)
        {
            _config = config;
            _snake = snake;
            _difficulty = difficulty;
        }
        
        public void Initialize()
        {
            _difficulty.OnStateChanged += OnDifficultyChanged;
        }

        public void Dispose()
        {
            _difficulty.OnStateChanged -= OnDifficultyChanged;
        }

        private void OnDifficultyChanged()
        {
            int difficulty = _difficulty.Current;
            float speed = _config.GetSnakeSpeed(difficulty);
            
            _snake.SetSpeed(speed);
        }
    }
}
