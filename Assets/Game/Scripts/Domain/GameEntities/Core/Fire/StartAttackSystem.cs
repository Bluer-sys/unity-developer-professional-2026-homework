using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Game
{
	[BurstCompile, UpdateAfter(typeof(UnitMoveSystem)), UpdateAfter(typeof(AmmoRecoverySystem))]
	public partial struct StartAttackSystem : ISystem
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

			foreach (var (request, requestEnabled, cooldown, health, team, range, transform, entity) in
					 SystemAPI.Query<RefRO<FireRequest>, EnabledRefRW<FireRequest>, RefRW<FireCooldown>,
						 RefRO<Health>, RefRO<Team>, RefRO<AttackDistance>, RefRW<LocalTransform>>()
						 .WithDisabled<AttackWindup>()
						 .WithEntityAccess())
			{
				Entity target = request.ValueRO.target;
				requestEnabled.ValueRW = false;

				if (cooldown.ValueRO.IsPlaying() || health.ValueRO.IsDead())
					continue;

				if (SystemAPI.TryGetComponent(entity, out Ammo ammo) && ammo.value <= 0)
					continue;

				if (!FireUseCase.TryAimAtTarget(entity, target, team.ValueRO, range.ValueRO, ref transform.ValueRW,
						teams, healths, transforms, projectilePrefabs))
					continue;

				ref AttackWindup windup = ref SystemAPI.GetComponentRW<AttackWindup>(entity).ValueRW;
				windup.target = target;
				windup.readyTime = now + windup.delay;
				SystemAPI.SetComponentEnabled<AttackWindup>(entity, true);
				SystemAPI.SetComponentEnabled<AttackStartedEvent>(entity, true);
				cooldown.ValueRW.ResetTime();
			}
		}
	}
}
