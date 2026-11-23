using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Game
{
	[BurstCompile, UpdateAfter(typeof(StartAttackSystem))]
	public partial struct AttackWindupSystem : ISystem
	{
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<GameState>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (!SystemAPI.GetSingleton<GameState>().IsRunning)
				return;

			var teams = SystemAPI.GetComponentLookup<Team>(true);
			var healths = SystemAPI.GetComponentLookup<Health>(true);
			var transforms = SystemAPI.GetComponentLookup<LocalTransform>(true);
			var projectilePrefabs = SystemAPI.GetComponentLookup<ProjectilePrefab>(true);
			double now = SystemAPI.Time.ElapsedTime;

			foreach (var (windup, health, team, range, transform, entity) in
					 SystemAPI.Query<RefRO<AttackWindup>, RefRO<Health>, RefRO<Team>,
						 RefRO<AttackDistance>, RefRW<LocalTransform>>().WithEntityAccess())
			{
				if (now < windup.ValueRO.readyTime)
					continue;

				Entity target = windup.ValueRO.target;
				SystemAPI.SetComponentEnabled<AttackWindup>(entity, false);

				if (health.ValueRO.IsDead() ||
					!FireUseCase.TryAimAtTarget(entity, target, team.ValueRO, range.ValueRO, ref transform.ValueRW,
						teams, healths, transforms, projectilePrefabs))
					continue;

				SystemAPI.SetComponent(entity, new AttackReadyRequest { target = target });
				SystemAPI.SetComponentEnabled<AttackReadyRequest>(entity, true);
			}
		}
	}
}
