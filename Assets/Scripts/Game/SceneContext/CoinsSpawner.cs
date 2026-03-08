using System;
using System.Collections.Generic;
using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

namespace Game.SceneContext
{
    public class CoinsSpawner : IInitializable, ICoinsSpawner
    {
        public event Action OnCoinsOver;
        
        private readonly CoinsPool _coinPool;
        private readonly IDifficulty _difficulty;
        private readonly IWorldBounds _world;

        private readonly Dictionary<Vector2Int, ICoin> _spawnedCoins = new();

        public CoinsSpawner(IDifficulty difficulty, CoinsPool coinPool, IWorldBounds world)
        {
            _difficulty = difficulty;
            _coinPool = coinPool;
            _world = world;
        }
        
        public void Initialize()
        {
            Next();
        }

        private void Spawn(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2Int position = _world.GetRandomPosition();
                
                if(_spawnedCoins.ContainsKey(position))
                    continue;
                
                ICoin coin = _coinPool.Spawn(position);

                _spawnedCoins.Add(position, coin);
            }
        }

        public bool TryTakeCoin(Vector2Int position, out ICoin coin)
        {
            bool hasCoin = _spawnedCoins.Remove(position, out coin);

            if (hasCoin)
            {
                _coinPool.Despawn((Coin)coin);
                
                if(_spawnedCoins.Count == 0)
                    Next();
            }
            
            return hasCoin;
        }

        private void Next()
        {
            if (!_difficulty.Next(out int difficulty))
            {
                OnCoinsOver?.Invoke();
                return;
            }

            Spawn(difficulty);
        }
    }
}
