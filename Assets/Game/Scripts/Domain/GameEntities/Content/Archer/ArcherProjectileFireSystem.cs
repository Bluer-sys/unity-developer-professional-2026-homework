using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
	[BurstCompile, UpdateAfter(typeof(AttackWindupSystem)), UpdateBefore(typeof(TakeDamageSystem))]
	public partial struct ArcherProjectileFireSystem : ISystem
	{
		private ComponentLookup<LocalTransform> _transformLookup;

		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<GameState>();
			state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
			_transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (!SystemAPI.GetSingleton<GameState>().IsRunning)
				return;

			_transformLookup.Update(ref state);
			EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
				.CreateCommandBuffer(state.WorldUnmanaged);

			foreach (var (request, ammo, team, transform, offset, prefab, entity) in
					 SystemAPI.Query<RefRO<AttackReadyRequest>, RefRW<Ammo>, RefRO<Team>,
						 RefRO<LocalTransform>, RefRO<FireOffset>, RefRO<ProjectilePrefab>>()
						 .WithAll<Archer>()
						 .WithEntityAccess())
			{
				Entity target = request.ValueRO.target;
				SystemAPI.SetComponentEnabled<AttackReadyRequest>(entity, false);

				if (ammo.ValueRO.value <= 0 || !_transformLookup.TryGetComponent(target, out LocalTransform targetTransform))
					continue;

				float3 firePoint = FireUseCase.GetFirePoint(transform.ValueRO, offset.ValueRO);
				TargetOffset aim = SystemAPI.GetComponent<TargetOffset>(prefab.ValueRO.value);
				float3 direction = math.normalizesafe(targetTransform.TransformPoint(aim.value) - firePoint, transform.ValueRO.Forward());
				LocalTransform projectileTransform = _transformLookup[prefab.ValueRO.value];
				projectileTransform.Position = firePoint;
				projectileTransform.Rotation = ProjectileUseCase.GetRotation(direction);

				ProjectileUseCase.SpawnProjectile(ref ecb, prefab.ValueRO, projectileTransform, team.ValueRO, target, entity);
				ammo.ValueRW.value--;
				SystemAPI.SetComponentEnabled<FireEvent>(entity, true);
			}
		}
	}
}
