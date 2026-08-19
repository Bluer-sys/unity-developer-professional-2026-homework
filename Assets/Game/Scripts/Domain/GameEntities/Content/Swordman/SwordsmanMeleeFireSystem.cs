using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
	[BurstCompile, UpdateAfter(typeof(AttackTargetSystem))]
	public partial struct SwordsmanMeleeFireSystem : ISystem
	{
		private ComponentLookup<Team> _teamLookup;
		private ComponentLookup<LocalTransform> _transformLookup;
		private ComponentLookup<AttackDistance> _attackDistanceLookup;
		private BufferLookup<TakeDamageRequest> _takeDamageRequests;

		public void OnCreate(ref SystemState state)
		{
			_teamLookup = SystemAPI.GetComponentLookup<Team>(true);
			_transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(false);
			_attackDistanceLookup = SystemAPI.GetComponentLookup<AttackDistance>(true);
			_takeDamageRequests = SystemAPI.GetBufferLookup<TakeDamageRequest>(false);
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			_teamLookup.Update(ref state);
			_transformLookup.Update(ref state);
			_attackDistanceLookup.Update(ref state);
			_takeDamageRequests.Update(ref state);

			foreach ((
						 EnabledRefRW<FireRequest> requestEnabled,
						 RefRO<FireRequest> requestValue,
						 RefRW<FireCooldown> cooldown,
						 RefRO<Health> health,
						 RefRO<Damage> damage,
						 EnabledRefRW<FireEvent> fireEvent,
						 Entity entity
					 ) in SystemAPI.Query<
							 EnabledRefRW<FireRequest>,
							 RefRO<FireRequest>,
							 RefRW<FireCooldown>,
							 RefRO<Health>,
							 RefRO<Damage>,
							 EnabledRefRW<FireEvent>>()
						 .WithAll<Swordsman>()
						 .WithAll<Team>()
						 .WithAll<AttackDistance>()
						 .WithAll<LocalTransform>()
						 .WithPresent<FireEvent>()
						 .WithEntityAccess())
			{
				requestEnabled.ValueRW = false;

				if (cooldown.ValueRO.IsPlaying() || health.ValueRO.IsDead())
					continue;

				if (!_teamLookup.TryGetComponent(entity, out Team team) || !_attackDistanceLookup.TryGetComponent(entity, out AttackDistance attackDistance) || !_transformLookup.TryGetComponent(entity, out LocalTransform transform))
					continue;

				Entity target = requestValue.ValueRO.target;

				if (target == Entity.Null || !SystemAPI.Exists(target) || !_transformLookup.TryGetComponent(target, out LocalTransform targetTransform))
					continue;

				TeamType myTeam = team.value;

				if (!_teamLookup.TryGetComponent(target, out Team targetTeam) || targetTeam.value == myTeam)
					continue;

				float3 delta = targetTransform.Position - transform.Position;
				float distance = attackDistance.value;

				if (math.lengthsq(delta) > distance * distance)
					continue;

				if (!_takeDamageRequests.TryGetBuffer(target, out DynamicBuffer<TakeDamageRequest> requests))
					continue;

				transform.Rotation = quaternion.LookRotationSafe(math.normalizesafe(delta), math.up());
				_transformLookup[entity] = transform;

				requests.Add(new TakeDamageRequest
				{
					damage = damage.ValueRO.value,
					instigator = entity
				});

				cooldown.ValueRW.ResetTime();
				fireEvent.ValueRW = true;
			}
		}
	}
}
