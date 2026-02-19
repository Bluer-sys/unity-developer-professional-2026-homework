using Game.GameContext;
using UnityEngine;

namespace Game.GameObjects
{
    public interface IWeapon
    {
        void Construct(BulletSpawner bulletSpawner);

        bool TryFire(Vector2 direction);

        void ResetCooldown(float fireCooldown);
    }
}
