using System;
using Modules;
using Zenject;

namespace Game.Gameplay
{
    public class SnakeSpeedController : IInitializable, IDisposable
    {
        private readonly ISnake _snake;
        private readonly IDifficulty _difficulty;

        public SnakeSpeedController(ISnake snake, IDifficulty difficulty)
        {
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
            _snake.SetSpeed(_difficulty.Current);
        }
    }
}
