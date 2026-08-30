using Fusion;
using UnityEngine;

namespace Game.Player
{
    public class PlayerJoinController : SimulationBehaviour, IPlayerJoined
    {
        [SerializeField] private GameObject _characterPrefab;
        [SerializeField] private SpawnPointService _spawnPointService;
        [SerializeField] private PlayersRoom _playersRoom;
        
        public void PlayerJoined(PlayerRef player)
        {
            if(!Runner.IsServer)
                return;

            Vector3 spawnPos = _spawnPointService.GetRandomSpawnPosition();
            NetworkObject character = Runner.Spawn(_characterPrefab, spawnPos, Quaternion.identity, player);
            
            Runner.SetPlayerObject(player, character);
            _playersRoom.JoinPlayer(character);
        }
    }
}
