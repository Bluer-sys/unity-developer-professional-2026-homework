using Fusion;
using Game.Core;
using UnityEngine;

namespace Game
{
    public sealed class DeathVfxComponent : NetworkBehaviour
    {
        [SerializeField] private GameObject _vfxPrefab;
        [SerializeField] private HealthComponent _healthComponent;

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            Instantiate(_vfxPrefab, transform.position, transform.rotation);
        }
    }
}
