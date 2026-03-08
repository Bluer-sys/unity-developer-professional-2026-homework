using System;
using Modules;
using Zenject;

namespace Game.PlayerContext.Input
{
    public class PlayerInputController : IInitializable, IDisposable
    {
        private readonly ISnake _snake;
        private readonly IPlayerInput _playerInput;

        public PlayerInputController(ISnake snake, IPlayerInput playerInput)
        {
            _snake = snake;
            _playerInput = playerInput;
        }

        public void Initialize()
        {
            _playerInput.OnHorizontalAxis += OnHorizontalAxis;
            _playerInput.OnVerticalAxis += OnVerticalAxis;
        }

        public void Dispose()
        {
            _playerInput.OnHorizontalAxis -= OnHorizontalAxis;
            _playerInput.OnVerticalAxis -= OnVerticalAxis;
        }

        private void OnHorizontalAxis(float axis)
        {
            _snake.Turn(axis > 0 ? SnakeDirection.RIGHT : SnakeDirection.LEFT);
        }

        private void OnVerticalAxis(float axis)
        {
            _snake.Turn(axis > 0 ? SnakeDirection.UP : SnakeDirection.DOWN);
        }
    }
}
