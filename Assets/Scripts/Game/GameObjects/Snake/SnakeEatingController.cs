using System;
using Game.GameContext;
using Modules;
using UnityEngine;
using Zenject;

namespace Game.GameObjects
{
    public class SnakeEatingController : IInitializable, IDisposable
    {
        private readonly ISnake _snake;
        private readonly IScore _score;
        private readonly ICoinsSpawner _coinsSpawner;
        private readonly IGameCycle _gameCycle;

        public SnakeEatingController(
                ISnake snake, 
                IScore score,
                ICoinsSpawner coinsSpawner,
                IGameCycle gameCycle
            )
        {
            _snake = snake;
            _score = score;
            _coinsSpawner = coinsSpawner;
            _gameCycle = gameCycle;
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
            _gameCycle.SetScore(_score.Current);
        }
    }
}
