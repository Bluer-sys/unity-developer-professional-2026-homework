using System;
using Modules;
using UnityEngine;

namespace Game.SceneContext
{
    public interface ICoinsSpawner
    {
        event Action OnCoinsOver;

        bool TryTakeCoin(Vector2Int position, out ICoin coin);
    }
}
