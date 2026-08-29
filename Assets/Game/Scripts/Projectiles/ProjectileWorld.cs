using Fusion;
using UnityEngine;

namespace Game.Projectiles
{
    public class ProjectileWorld : NetworkBehaviour
    {
        private const int Capacity = 32;

        [SerializeField]
        private ProjectileConfig _config;
        
        [Networked, Capacity(Capacity)]
        private NetworkArray<Projectile> _projectiles { get; }
        public int Length => Capacity;

        public bool TrySpawn(Vector3 position, Quaternion rotation)
        {
            if (!FindFreeSlot(out int freeIndex))
                return false;
            
            var projectile = new Projectile
            {
                StartTick = Runner.Tick,
                Position = position,
                Rotation = rotation
            };

            _projectiles.Set(freeIndex, projectile);
            
            return true;
        }

        public override void FixedUpdateNetwork()
        {
            for (int i = 0; i < Capacity; i++)
            {
                ref Projectile projectile = ref _projectiles.GetRef(i);
                
                if (!projectile.IsAlive)
                    continue;

                _config.OnSimulate(ref projectile, Runner.LocalPlayer, Runner, out bool finished);

                if (finished)
                    projectile = default;
            }
        }

        public Projectile Get(int index)
        {
            return _projectiles[index];
        }

        private bool FindFreeSlot(out int index)
        {
            for (int i = 0; i < Capacity; i++)
            {
                Projectile projectile = _projectiles[i];

                if (!projectile.IsAlive)
                {
                    index = i;
                    return true;
                }
            }

            index = -1;
            return false;
        }
    }
}
