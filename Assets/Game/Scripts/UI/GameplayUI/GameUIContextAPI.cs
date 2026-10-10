using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.UI
{
    [EntityExtensionsAPI]
    public static partial class GameUIContextAPI
    {
        public static readonly ValueKey<IGameUIContext, IValue<Vector2>> MoveInput = new(nameof(MoveInput));
        public static readonly ValueKey<IGameUIContext, IValue<Vector2>> AimInput = new(nameof(AimInput));
    }
}
