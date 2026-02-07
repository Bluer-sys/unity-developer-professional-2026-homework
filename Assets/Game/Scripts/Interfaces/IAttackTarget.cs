using UnityEngine;

namespace Game.Interfaces
{
    public interface IAttackTarget
    {
        Transform Transform { get; }
        bool IsDead { get; }
    }
}
