using UnityEngine;

namespace Game.GameObjects.Weapon
{
    public interface IWeapon
    {
        bool TryFire(Vector2 direction);
    }
}
