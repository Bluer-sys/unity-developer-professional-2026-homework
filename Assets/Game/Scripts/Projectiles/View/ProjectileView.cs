using Fusion;
using UnityEngine;

namespace Game.Projectiles.View
{
    public class ProjectileView : MonoBehaviour
    {
        [SerializeField] private ProjectileConfig _config;

        public void OnSpawn(
                in Projectile projectile,
                PlayerRef player,
                NetworkRunner runner
            )
        {
            UpdatePosition(in projectile, player, runner);
        }

        public void OnRender(
                in Projectile projectile,
                PlayerRef player,
                NetworkRunner runner
            )
        {
            UpdatePosition(in projectile, player, runner);
        }
        
        private void UpdatePosition(in Projectile projectile, PlayerRef player, NetworkRunner runner)
        {
            transform.position = _config.GetRenderPosition(in projectile, player, runner);
        }
    }
}
