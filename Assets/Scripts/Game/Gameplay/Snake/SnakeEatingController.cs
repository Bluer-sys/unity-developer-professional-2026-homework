using System;
using Modules;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public class SnakeEatingController : IInitializable, IDisposable
    {
        private readonly ISnake _snake;
        private readonly IScore _score;
        private readonly ICoinsSpawner _coinsSpawner;

        public SnakeEatingController(
                ISnake snake, 
                IScore score,
                ICoinsSpawner coinsSpawner
            )
        {
            _snake = snake;
            _score = score;
            _coinsSpawner = coinsSpawner;
        }

        public void Initialize()
        {
            _snake.OnMoved += OnSnakeMoved;
        }

        public void Dispose()
        {
            _snake.OnMoved -= OnSnakeMoved;
        }

        private void OnSnakeMoved(Vector2Int headPosition)
        {
            if (_coinsSpawner.TryTakeCoin(headPosition, out ICoin coin))
                Collect(coin);
        }

        private void Collect(ICoin coin)
        {
            _score.Add(coin.Score);
            _snake.Expand(coin.Bones);
        }
    }
}
