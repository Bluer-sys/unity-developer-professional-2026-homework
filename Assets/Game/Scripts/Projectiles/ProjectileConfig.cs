using Fusion;
using UnityEngine;

namespace Game.Projectiles
{
    [CreateAssetMenu(fileName = "New Projectile Config", menuName = "Game/Projectiles/Projectile Config")]
    public class ProjectileConfig : ScriptableObject
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _lifetime;

        public void OnSimulate(ref Projectile projectile, PlayerRef player, NetworkRunner runner, out bool finished)
        {
            finished = false;

            float deltaTime = runner.DeltaTime;
            float currentTime = (runner.Tick - projectile.StartTick) * deltaTime;

            if (currentTime > _lifetime)
            {
                finished = true;
                return;
            }

            float previousTime = Mathf.Max(0, currentTime - deltaTime);
            Vector3 direction = projectile.Direction;
            Vector3 position = projectile.Position + direction * previousTime * _speed;
            
            bool wasHit = runner
                .GetPhysicsScene()
                .Raycast(position, direction, out RaycastHit hit, _speed * deltaTime, _layerMask, QueryTriggerInteraction.Ignore);

            if (wasHit)
            {
                DealDamage(hit.collider, player);
                finished = true;
            }
        }

        public Vector3 GetRenderPosition(in Projectile projectile, PlayerRef player, NetworkRunner runner)
        {
            float deltaTime = runner.DeltaTime;
            float spawnTime = projectile.StartTick * deltaTime;

            float renderTime = runner.IsServer
                                   ? runner.LocalRenderTime + deltaTime
                                   : runner.RemoteRenderTime + deltaTime;

            float t = renderTime - spawnTime;
            return projectile.Position + projectile.Direction * t * _speed;
        }

        private bool DealDamage(Collider collider, PlayerRef player)
        {
            var target = collider.GetComponentInParent<NetworkObject>();

            if (target == null ||
                target.InputAuthority == player || 
                !target.TryGetBehaviour(out TakeDamageComponent takeDamageComponent))
                return false;

            takeDamageComponent.TakeDamage(new TakeDamageArgs(_damage));
            return true;
        }
    }
}
