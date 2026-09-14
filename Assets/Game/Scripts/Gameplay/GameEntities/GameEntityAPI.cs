using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    [EntityExtensionsAPI]
    public static partial class GameEntityAPI
    {
        public static readonly ValueKey<IGameEntity, Transform> Transform = new(nameof(Transform));
        public static readonly ValueKey<IGameEntity, IReactiveVariable<int>> Health = new(nameof(Health));
        public static readonly ValueKey<IGameEntity, IValue<int>> MaxHealth = new(nameof(MaxHealth));
        public static readonly ValueKey<IGameEntity, IEvent<DamageArgs>> TakeDamageEvent = new(nameof(TakeDamageEvent));
        public static readonly ValueKey<IGameEntity, IEvent<DamageArgs>> DeathEvent = new(nameof(DeathEvent));
        public static readonly ValueKey<IGameEntity, Animator> Animator = new(nameof(Animator));
        public static readonly ValueKey<IGameEntity, Rigidbody> Rigidbody = new(nameof(Rigidbody));
        public static readonly ValueKey<IGameEntity, ISignal<Vector3, float>> RootMotion = new(nameof(RootMotion));
        public static readonly ValueKey<IGameEntity, IVariable<Vector3>> MovementDirection = new(nameof(MovementDirection));
        public static readonly ValueKey<IGameEntity, IValue<float>> RootMotionMultiplier = new(nameof(RootMotionMultiplier));
        public static readonly ValueKey<IGameEntity, IVariable<Vector3>> RotationDirection = new(nameof(RotationDirection));
        public static readonly ValueKey<IGameEntity, IValue<float>> RotationSpeed = new(nameof(RotationSpeed));
        public static readonly ValueKey<IGameEntity, IReactiveVariable<bool>> IsAiming = new(nameof(IsAiming));
    }
}
