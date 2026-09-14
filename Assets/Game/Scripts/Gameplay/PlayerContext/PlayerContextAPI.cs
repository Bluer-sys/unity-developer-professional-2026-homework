using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    [EntityExtensionsAPI]
    public static partial class PlayerContextAPI
    {
        public static readonly ValueKey<IPlayerContext, IGameEntity> Character = new(nameof(Character));
        public static readonly ValueKey<IPlayerContext, IValue<Vector2>> MoveInput = new(nameof(MoveInput));
        public static readonly ValueKey<IPlayerContext, IValue<Vector2>> AimInput = new(nameof(AimInput));
    }
}
