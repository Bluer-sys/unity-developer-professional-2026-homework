using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    [EntityExtensionsAPI]
    public static partial class GameEntityAPI
    {
        public static readonly TagKey<IGameEntity> Player = new(nameof(Player));

        public static readonly ValueKey<IGameEntity, Transform> Transform = new(nameof(Transform));
        public static readonly ValueKey<IGameEntity, IReactiveVariable<int>> Health = new(nameof(Health));
        public static readonly ValueKey<IGameEntity, IValue<int>> MaxHealth = new(nameof(MaxHealth));
        public static readonly ValueKey<IGameEntity, IEvent<DamageArgs>> TakeDamageEvent = new(nameof(TakeDamageEvent));
        public static readonly ValueKey<IGameEntity, IEvent<DamageArgs>> DeathEvent = new(nameof(DeathEvent));
        public static readonly ValueKey<IGameEntity, Animator> Animator = new(nameof(Animator));
        public static readonly ValueKey<IGameEntity, Rigidbody> Rigidbody = new(nameof(Rigidbody));
        public static readonly ValueKey<IGameEntity, IVariable<IGameEntity>> Target = new(nameof(Target));
        public static readonly ValueKey<IGameEntity, IPredicate<IGameEntity>> TargetPredicate = new(nameof(TargetPredicate));
        public static readonly ValueKey<IGameEntity, ISignal<Vector3, float>> RootMotion = new(nameof(RootMotion));
        public static readonly ValueKey<IGameEntity, IVariable<Vector3>> MovementDirection = new(nameof(MovementDirection));
        public static readonly ValueKey<IGameEntity, IValue<float>> RootMotionMultiplier = new(nameof(RootMotionMultiplier));
        public static readonly ValueKey<IGameEntity, IVariable<Vector3>> RotationDirection = new(nameof(RotationDirection));
        public static readonly ValueKey<IGameEntity, IValue<float>> RotationSpeed = new(nameof(RotationSpeed));
        public static readonly ValueKey<IGameEntity, IReactiveVariable<bool>> IsAiming = new(nameof(IsAiming));
        public static readonly ValueKey<IGameEntity, IGameEntity> Weapon = new(nameof(Weapon));
        public static readonly ValueKey<IGameEntity, IRequest> AttackRequest = new(nameof(AttackRequest));
        public static readonly ValueKey<IGameEntity, ICooldown> AttackDelay = new(nameof(AttackDelay));
        public static readonly ValueKey<IGameEntity, ICooldown> AttackDuration = new(nameof(AttackDuration));
        public static readonly ValueKey<IGameEntity, IExpression<bool>> AttackCondition = new(nameof(AttackCondition));
        public static readonly ValueKey<IGameEntity, IAction> AttackAction = new(nameof(AttackAction));
        public static readonly ValueKey<IGameEntity, IEvent> AttackStartedEvent = new(nameof(AttackStartedEvent));
        public static readonly ValueKey<IGameEntity, IEvent> AttackCancelledEvent = new(nameof(AttackCancelledEvent));
        public static readonly ValueKey<IGameEntity, ICooldown> FirstAttackDelay = new(nameof(FirstAttackDelay));
        public static readonly ValueKey<IGameEntity, Transform> FirePoint = new(nameof(FirePoint));
        public static readonly ValueKey<IGameEntity, Transform> HitPoint = new(nameof(HitPoint));
        public static readonly ValueKey<IGameEntity, IValue<float>> HitRadius = new(nameof(HitRadius));
        public static readonly ValueKey<IGameEntity, IValue<float>> AttackDistance = new(nameof(AttackDistance));
        public static readonly ValueKey<IGameEntity, IValue<LayerMask>> HitMask = new(nameof(HitMask));
        public static readonly ValueKey<IGameEntity, IReactiveVariable<int>> Ammo = new(nameof(Ammo));
        public static readonly ValueKey<IGameEntity, ICooldown> FireCooldown = new(nameof(FireCooldown));
        public static readonly ValueKey<IGameEntity, IValue<float>> SpreadAngle = new(nameof(SpreadAngle));
        public static readonly ValueKey<IGameEntity, IEvent> FireEvent = new(nameof(FireEvent));
        public static readonly ValueKey<IGameEntity, IEntityPool<IGameEntity>> BulletPool = new(nameof(BulletPool));
        public static readonly ValueKey<IGameEntity, IVariable<IGameEntity>> Owner = new(nameof(Owner));
        public static readonly ValueKey<IGameEntity, IValue<int>> Damage = new(nameof(Damage));
        public static readonly ValueKey<IGameEntity, IValue<float>> BulletSpeed = new(nameof(BulletSpeed));
        public static readonly ValueKey<IGameEntity, IValue<float>> BulletRadius = new(nameof(BulletRadius));
        public static readonly ValueKey<IGameEntity, ICooldown> Lifetime = new(nameof(Lifetime));
        public static readonly ValueKey<IGameEntity, IVariable<bool>> IsFlying = new(nameof(IsFlying));
        public static readonly ValueKey<IGameEntity, IVariable<bool>> IsCollected = new(nameof(IsCollected));
        public static readonly ValueKey<IGameEntity, IExpression<IGameEntity, bool>> PickupCondition = new(nameof(PickupCondition));
        public static readonly ValueKey<IGameEntity, IAction<IGameEntity>> PickupAction = new(nameof(PickupAction));
        public static readonly ValueKey<IGameEntity, IEvent> PickupEvent = new(nameof(PickupEvent));
        public static readonly ValueKey<IGameEntity, IValue<int>> PickupAmount = new(nameof(PickupAmount));
    }
}
