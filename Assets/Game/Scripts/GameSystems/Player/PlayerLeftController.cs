using Fusion;
using UnityEngine;

namespace Game.Player
{
    public sealed class PlayerLeftController : SimulationBehaviour, IPlayerLeft
    {
        [SerializeField] private PlayersRoom _playersRoom;
        
        void IPlayerLeft.PlayerLeft(PlayerRef player)
        {
            if (!Runner.IsServer || !Runner.TryGetPlayerObject(player, out NetworkObject character))
                return;

            _playersRoom.LeavePlayer(character);
            Runner.Despawn(character);
        }
    }
}
