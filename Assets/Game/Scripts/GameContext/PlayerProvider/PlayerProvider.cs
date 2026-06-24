namespace Game
{
    public class PlayerProvider : IPlayerProvider
    {
        public IGameEntity Player { get; }

        public PlayerProvider(IGameEntity player)
        {
            Player = player;
        }
    }
}
