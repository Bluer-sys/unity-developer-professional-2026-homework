using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
	[BurstCompile, UpdateAfter(typeof(AttackTargetSystem))]
	public partial struct UnitMoveSystem : ISystem
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

			state.Dependency = new MoveJob
			{
				DeltaTime = SystemAPI.Time.DeltaTime
			}.ScheduleParallel(state.Dependency);
		}

		[WithPresent(typeof(MoveEvent)), WithAll(typeof(Unit)), BurstCompile]
		public partial struct MoveJob : IJobEntity
		{
			public float DeltaTime;

			private void Execute(
					EnabledRefRW<MoveRequest> requestEnabled,
					EnabledRefRW<MoveEvent> eventEnabled,
					ref MoveRequest request,
					ref LocalTransform transform,
					in MoveSpeed moveSpeed,
					in RotationSpeed rotationSpeed,
					in Health health
				)
			{
				requestEnabled.ValueRW = false;

				float3 direction = request.direction;

				if (math.all(direction == float3.zero) || health.IsDead())
					return;

				float distance = MoveUseCase.MoveStep(ref transform, in direction, in moveSpeed, DeltaTime, request.maxDistance);
				RotationUseCase.RotateStep(ref transform, in direction, in rotationSpeed, DeltaTime);
				eventEnabled.ValueRW = distance > 0f;
			}
		}
	}
}
