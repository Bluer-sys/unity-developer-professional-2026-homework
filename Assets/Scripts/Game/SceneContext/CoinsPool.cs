using Modules;
using UnityEngine;
using Zenject;

namespace Game.SceneContext
{
    public class CoinsPool : MonoMemoryPool<Vector2Int, Coin>
    {
        protected override void Reinitialize(Vector2Int position, Coin item)
        {
            item.Generate();
            item.Position = position;
        }
    }
}
