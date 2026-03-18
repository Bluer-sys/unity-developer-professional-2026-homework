using System;
using Modules;
using UnityEngine;

namespace Game.Gameplay
{
    public interface ICoinsSpawner
    {
        event Action OnAllCollected;

        bool TryTakeCoin(Vector2Int position, out ICoin coin);
        void Spawn(int difficulty);
    }
}
