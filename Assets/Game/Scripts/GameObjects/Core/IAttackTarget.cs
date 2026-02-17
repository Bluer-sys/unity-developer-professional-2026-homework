using UnityEngine;

namespace Game.GameObjects.Core
{
    public interface IAttackTarget
    {
        Transform Transform { get; }
        bool IsDead { get; }
    }
}
