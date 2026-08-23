using Fusion;

namespace Game.Player
{
    public sealed class PlayerLeftController : SimulationBehaviour, IPlayerLeft
    {
        void IPlayerLeft.PlayerLeft(PlayerRef player)
        {
            if (Runner.IsServer && Runner.TryGetPlayerObject(player, out NetworkObject character)) 
                Runner.Despawn(character);
        }
    }
}
