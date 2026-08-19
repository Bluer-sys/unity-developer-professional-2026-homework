using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
	[BurstCompile, UpdateAfter(typeof(AttackTargetSystem))]
	public partial struct ArcherProjectileFireSystem : ISystem
	{
		private ComponentLookup<LocalTransform> _transformLookup;
		private ComponentLookup<Team> _teamLookup;
		private ComponentLookup<AttackDistance> _attackDistanceLookup;
		private ComponentLookup<ProjectilePrefab> _projectilePrefabLookup;
		private ComponentLookup<FireOffset> _fireOffsetLookup;

		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
			_transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(false);
			_teamLookup = SystemAPI.GetComponentLookup<Team>(true);
			_attackDistanceLookup = SystemAPI.GetComponentLookup<AttackDistance>(true);
			_projectilePrefabLookup = SystemAPI.GetComponentLookup<ProjectilePrefab>(true);
			_fireOffsetLookup = SystemAPI.GetComponentLookup<FireOffset>(true);
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			_transformLookup.Update(ref state);
			_teamLookup.Update(ref state);
			_attackDistanceLookup.Update(ref state);
			_projectilePrefabLookup.Update(ref state);
			_fireOffsetLookup.Update(ref state);

			EntityCommandBuffer ecb = SystemAPI
				.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
				.CreateCommandBuffer(state.WorldUnmanaged);

			foreach ((
						 EnabledRefRW<FireRequest> requestEnabled,
						 RefRO<FireRequest> requestValue,
						 RefRW<FireCooldown> cooldown,
						 RefRW<Ammo> ammo,
						 RefRO<Health> health,
						 EnabledRefRW<FireEvent> fireEvent,
						 Entity entity
					 ) in SystemAPI.Query<
							 EnabledRefRW<FireRequest>,
							 RefRO<FireRequest>,
							 RefRW<FireCooldown>,
							 RefRW<Ammo>,
							 RefRO<Health>,
							 EnabledRefRW<FireEvent>>()
						 .WithAll<Archer>()
						 .WithAll<Team>()
						 .WithAll<AttackDistance>()
						 .WithAll<LocalTransform>()
						 .WithAll<ProjectilePrefab>()
						 .WithAll<FireOffset>()
						 .WithPresent<FireEvent>()
						 .WithEntityAccess())
			{
				requestEnabled.ValueRW = false;

				if (cooldown.ValueRO.IsPlaying() || health.ValueRO.IsDead() || ammo.ValueRO.value <= 0)
					continue;

				if (!_teamLookup.TryGetComponent(entity, out Team team) || !_attackDistanceLookup.TryGetComponent(entity, out AttackDistance attackDistance) || !_transformLookup.TryGetComponent(entity, out LocalTransform transform) || !_projectilePrefabLookup.TryGetComponent(entity, out ProjectilePrefab projectilePrefab) || !_fireOffsetLookup.TryGetComponent(entity, out FireOffset fireOffset))
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

				transform.Rotation = quaternion.LookRotationSafe(math.normalizesafe(delta), math.up());
				_transformLookup[entity] = transform;

				ProjectileUseCase.SpawnProjectile(ref ecb,
					projectilePrefab,
					transform,
					fireOffset,
					team,
					target,
					entity);

				cooldown.ValueRW.ResetTime();
				ammo.ValueRW.value--;
				fireEvent.ValueRW = true;
			}
		}
	}
}
