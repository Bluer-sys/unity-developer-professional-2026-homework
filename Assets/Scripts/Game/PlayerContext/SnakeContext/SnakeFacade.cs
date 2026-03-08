using System;
using Modules;
using UnityEngine;
using Zenject;

namespace Game.PlayerContext.SnakeContext
{
    public class SnakeFacade : MonoBehaviour, IInitializable, IDisposable, ISnakeFacade
    {
        public event Action OnCollided;
        
        private ISnake _snake;
        private ISnakeHead _snakeHead;

        [Inject]
        private void Construct(ISnake snake, ISnakeHead snakeHead)
        {
            _snake = snake;
            _snakeHead = snakeHead;
        }

        public void Initialize()
        {
            _snake.OnSelfCollided += OnCollided;
            _snakeHead.OnCollided += OnCollided;
        }

        public void Dispose()
        {
            _snake.OnSelfCollided -= OnCollided;
            _snakeHead.OnCollided -= OnCollided;
        }

        public void SetActive(bool isActive)
        {
            _snake.SetActive(isActive);
        }
    }
}
