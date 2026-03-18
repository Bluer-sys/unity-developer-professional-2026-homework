using System;
using System.Collections.Generic;
using Modules;
using SnakeGame;
using UnityEngine;

namespace Game.GameContext
{
    public class CoinsSpawner : ICoinsSpawner
    {
        public event Action OnAllCollected;
        
        private readonly CoinsPool _coinPool;
        private readonly IWorldBounds _world;

        private readonly Dictionary<Vector2Int, ICoin> _spawnedCoins = new();

        public CoinsSpawner(CoinsPool coinPool, IWorldBounds world)
        {
            _coinPool = coinPool;
            _world = world;
        }
        
        public void Spawn(int count)
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
            if(_spawnedCoins.Count == 0)
            {
                coin = null;
                return false;
            }

            bool hasCoin = _spawnedCoins.Remove(position, out coin);

            if (hasCoin)
            {
                _coinPool.Despawn((Coin)coin);
                
                if(_spawnedCoins.Count == 0)
                    OnAllCollected?.Invoke();
            }
            
            return hasCoin;
        }
    }
}
