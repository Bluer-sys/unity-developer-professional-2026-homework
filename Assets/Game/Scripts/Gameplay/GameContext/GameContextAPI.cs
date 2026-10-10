using Atomic.Elements;
using Atomic.Entities;

namespace Game.Gameplay
{
    [EntityExtensionsAPI]
    public static partial class GameContextAPI
    {
        public static readonly ValueKey<IGameContext, IGameEntity> Character = new(nameof(Character));
        public static readonly ValueKey<IGameContext, IEntityPool<IGameEntity>> BulletPool = new(nameof(BulletPool));
        public static readonly ValueKey<IGameContext, IReactiveVariable<int>> KillCount = new(nameof(KillCount));
    }
}
