using System;
using Game.SceneContext;
using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

namespace Game.PlayerContext.SnakeContext
{
    public class SnakeHead : IInitializable, IDisposable, ISnakeHead
    {
        public event Action OnCollided;

        private readonly ISnake _snake;
        private readonly IScore _score;
        private readonly ICoinsSpawner _coinsSpawner;
        private readonly IWorldBounds _world;

        public SnakeHead(
            ISnake snake, 
            IScore score,
            ICoinsSpawner coinsSpawner, 
            IWorldBounds world)
        {
            _snake = snake;
            _score = score;
            _coinsSpawner = coinsSpawner;
            _world = world;
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
            {
                Collect(coin);
            }
            else if(!_world.IsInBounds(headPosition))
            {
                OnCollided?.Invoke();
            }
        }

        private void Collect(ICoin coin)
        {
            _score.Add(coin.Score);
            _snake.Expand(coin.Bones);
        }
    }
}
