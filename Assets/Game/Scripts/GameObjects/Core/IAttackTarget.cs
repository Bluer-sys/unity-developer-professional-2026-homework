using UnityEngine;

namespace Game.GameObjects
{
    public interface IAttackTarget
    {
        Transform Transform { get; }
        bool IsDead { get; }
    }
}
