using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
	[BurstCompile, UpdateAfter(typeof(UpdateSpatialHashSystem))]
	public partial struct DetectTargetSystem : ISystem
	{
		private ComponentLookup<LocalTransform> _transformLookup;
		private ComponentLookup<Team> _teamLookup;
		private ComponentLookup<Health> _healthLookup;

		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<GameState>();

			_transformLookup = state.GetComponentLookup<LocalTransform>(true);
			_teamLookup = state.GetComponentLookup<Team>(true);
			_healthLookup = state.GetComponentLookup<Health>(true);

			state.RequireForUpdate<SpatialHashData>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (!SystemAPI.GetSingleton<GameState>().IsRunning)
				return;

			_transformLookup.Update(ref state);
			_teamLookup.Update(ref state);
			_healthLookup.Update(ref state);

			state.Dependency = new DetectJob
			{
				DeltaTime = SystemAPI.Time.DeltaTime,
				SpatialHash = SystemAPI.GetSingleton<SpatialHashData>(),
				TransformLookup = _transformLookup,
				TeamLookup = _teamLookup,
				HealthLookup = _healthLookup
			}.ScheduleParallel(state.Dependency);
		}

		[BurstCompile, WithAll(typeof(Unit))]
		public partial struct DetectJob : IJobEntity
		{
			public float DeltaTime;

			[NativeDisableUnsafePtrRestriction]
			public SpatialHashData SpatialHash;

			[ReadOnly]
			public ComponentLookup<LocalTransform> TransformLookup;

			[ReadOnly]
			public ComponentLookup<Team> TeamLookup;

			[ReadOnly]
			public ComponentLookup<Health> HealthLookup;

			private void Execute(
					Entity entity,
					in LocalTransform transform,
					in Team team,
					in Health health,
					in DetectionRadius detectionRadius,
					in EnemyBaseTarget enemyBaseTarget,
					ref DetectionCooldown cooldown,
					ref TargetEntity target
				)
			{
				if (health.IsDead())
				{
					target.value = Entity.Null;
					return;
				}

				var condition = new IsEnemyPredicate(entity,
					team.value,
					TeamLookup,
					HealthLookup);

				if (target.value != Entity.Null &&
					(!condition.Invoke(target.value) ||
					 !TransformLookup.TryGetComponent(target.value, out LocalTransform targetTransform) ||
					 (target.value != enemyBaseTarget.value &&
					  math.distancesq(transform.Position, targetTransform.Position) > detectionRadius.value * detectionRadius.value)))
					target.value = Entity.Null;

				cooldown.time = math.max(cooldown.time - DeltaTime, 0f);

				if (cooldown.time <= 0f)
				{
					target.value = SpatialHash.FindClosest(transform.Position,
						detectionRadius.value,
						in condition,
						TransformLookup);
					cooldown.time = cooldown.duration;
				}

				if (target.value != Entity.Null)
					return;

				Entity baseEntity = enemyBaseTarget.value;

				if (baseEntity != Entity.Null && condition.Invoke(baseEntity) && TransformLookup.HasComponent(baseEntity))
					target.value = baseEntity;
			}
		}
	}
}
