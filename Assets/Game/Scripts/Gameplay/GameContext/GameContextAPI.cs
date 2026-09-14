using Atomic.Entities;

namespace Game.Gameplay
{
    [EntityExtensionsAPI]
    public static partial class GameContextAPI
    {
        public static readonly ValueKey<IGameContext, IPlayerContext> PlayerContext = new(nameof(PlayerContext));
    }
}
