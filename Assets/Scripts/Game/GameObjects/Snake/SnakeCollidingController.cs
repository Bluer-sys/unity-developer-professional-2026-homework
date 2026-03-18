using System;
using Game.GameContext;
using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

namespace Game.GameObjects
{
    public class SnakeCollidingController : IInitializable, IDisposable
    {
        private readonly ISnake _snake;
        private readonly IWorldBounds _world;
        private readonly IGameCycle _gameCycle;

        public SnakeCollidingController(
            ISnake snake, 
            IWorldBounds world,
            IGameCycle gameCycle)
        {
            _snake = snake;
            _world = world;
            _gameCycle = gameCycle;
        }

        public void Initialize()
        {
            _snake.OnMoved += OnSnakeMoved;
            _snake.OnSelfCollided += SetFailed;
        }

        public void Dispose()
        {
            _snake.OnMoved -= OnSnakeMoved;
            _snake.OnSelfCollided -= SetFailed;
        }

        private void OnSnakeMoved(Vector2Int headPosition)
        {
            if (!_world.IsInBounds(headPosition))
                SetFailed();
        }

        private void SetFailed()
        {
            _gameCycle.SetFailed();
            _snake.SetActive(false);
        }
    }
}
