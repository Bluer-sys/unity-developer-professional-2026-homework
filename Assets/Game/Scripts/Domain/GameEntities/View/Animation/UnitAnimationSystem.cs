using SnivelerCode.GpuAnimation.Runtime.Components;
using SnivelerCode.GpuAnimation.Runtime.Utils;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Game
{
	[BurstCompile, UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial struct UnitAnimationSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			state.Dependency = new AnimationJob
			{
				MoveEvents = SystemAPI.GetComponentLookup<MoveEvent>(true),
				AttackStarts = SystemAPI.GetComponentLookup<AttackStartedEvent>(true),
				Windups = SystemAPI.GetComponentLookup<AttackWindup>(true),
				FireEvents = SystemAPI.GetComponentLookup<FireEvent>(true),
				DamageEvents = SystemAPI.GetBufferLookup<TakeDamageEvent>(true),
				DeathEvents = SystemAPI.GetComponentLookup<DeathEvent>(true),
				DeadCooldowns = SystemAPI.GetComponentLookup<DeadCooldown>(true)
			}.ScheduleParallel(state.Dependency);
		}

		[BurstCompile]
		private partial struct AnimationJob : IJobEntity
		{
			[ReadOnly] public ComponentLookup<MoveEvent> MoveEvents;
			[ReadOnly] public ComponentLookup<AttackStartedEvent> AttackStarts;
			[ReadOnly] public ComponentLookup<AttackWindup> Windups;
			[ReadOnly] public ComponentLookup<FireEvent> FireEvents;
			[ReadOnly] public BufferLookup<TakeDamageEvent> DamageEvents;
			[ReadOnly] public ComponentLookup<DeathEvent> DeathEvents;
			[ReadOnly] public ComponentLookup<DeadCooldown> DeadCooldowns;

			private void Execute(in UnitAnimation animation, DynamicBuffer<AnimatorParameterData> parameters)
			{
				Entity unit = animation.unit;
				if (DeathEvents.IsComponentEnabled(unit))
				{
					animation.isMoving.Value(0).Apply(parameters);
					animation.fire.Value(0).Apply(parameters);
					animation.takeDamage.Value(0).Apply(parameters);
					animation.death.Value(1).Apply(parameters);
					return;
				}

				if (DeadCooldowns.IsComponentEnabled(unit))
					return;

				animation.isMoving.Value(MoveEvents.IsComponentEnabled(unit) ? 1 : 0).Apply(parameters);

				if (AttackStarts.IsComponentEnabled(unit))
				{
					animation.takeDamage.Value(0).Apply(parameters);
					animation.fire.Value(1).Apply(parameters);
				}
				else if (!DamageEvents[unit].IsEmpty && parameters[animation.fire].Value == 0)
				{
					if (Windups.IsComponentEnabled(unit) || FireEvents.IsComponentEnabled(unit))
						return;

					animation.takeDamage.Value(1).Apply(parameters);
				}
			}
		}
	}
}
