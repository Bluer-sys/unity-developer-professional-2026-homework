using Fusion;
using UnityEngine;

namespace Game.Player
{
    public class PlayerJoinController : SimulationBehaviour, IPlayerJoined
    {
        [SerializeField] private GameObject _characterPrefab;
        [SerializeField] private SpawnPointService _spawnPointService;
        
        public void PlayerJoined(PlayerRef player)
        {
            if(!Runner.IsServer)
                return;

            NetworkObject character = Runner.Spawn(_characterPrefab, _spawnPointService.GetRandomSpawnPosition(), Quaternion.identity, player);
            Runner.SetPlayerObject(player, character);
        }
    }
}
