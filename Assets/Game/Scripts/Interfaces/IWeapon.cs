using UnityEngine;

namespace Game.Interfaces
{
    public interface IWeapon
    {
        bool TryFire(Vector2 direction);
    }
}
