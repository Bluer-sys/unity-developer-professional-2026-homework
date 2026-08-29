using Fusion;
using UnityEngine;

namespace Game.Projectiles.View
{
    public class ProjectileWorldView : NetworkBehaviour
    {
        [SerializeField] private ProjectileWorld _world;
        [SerializeField] private ProjectileView _viewPrefab;

        [SerializeField] private Transform _container;

        private ProjectileView[] _projectileViews;
        
        public override void Spawned()
        {
            _projectileViews = new ProjectileView[_world.Length];   
        }

        public override void Render()
        {
            PlayerRef player = _world.Object.InputAuthority;
            NetworkRunner runner = Runner;

            for (int i = 0; i < _world.Length; i++)
            {
                Projectile projectile = _world.Get(i);
                bool hasProjectile = projectile.IsAlive;

                ref ProjectileView view = ref _projectileViews[i];
                bool hasView = view != null;

                if (hasProjectile && !hasView)
                {
                    view = Instantiate(_viewPrefab, projectile.Position, projectile.Rotation, _container);
                    view.OnSpawn(in projectile, player, runner);
                }
                else if (hasView && !hasProjectile)
                {
                    Destroy(view.gameObject);
                    view = null;
                }
                else if (hasProjectile)
                {
                    view.OnRender(in projectile, player, runner);
                }
            }
        }
    }
}
