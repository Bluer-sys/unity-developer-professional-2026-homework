using System.Collections.Generic;
using Fusion;
using Game.Core;
using UnityEngine;

namespace Game.Player
{
    public class PlayersRoom : SimulationBehaviour
    {
        private const int NecessaryPlayers = 2;
        
        [SerializeField] private bool _waitAllPlayers;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private GameResult _gameResult;

        private readonly List<NetworkObject> _players = new(NecessaryPlayers);

        public override void FixedUpdateNetwork()
        {
            foreach (NetworkObject player in _players)
            {
                var healthComponent = player.GetBehaviour<HealthComponent>();
                
                if(healthComponent.IsDead)
                    _gameResult.Change(GameResult.Result.Lose);
            }
        }

        public void JoinPlayer(NetworkObject player)
        {
            _players.Add(player);

            TryLaunchGame();
        }

        public void LeavePlayer(NetworkObject player)
        {
            _players.Remove(player);
        }

        private void TryLaunchGame()
        {
            if (_waitAllPlayers)
            {
                if (_players.Count == NecessaryPlayers)
                    _enemySpawner.Launch();
            }
            else
            {
                _enemySpawner.Launch();
            }
        }
    }
}
