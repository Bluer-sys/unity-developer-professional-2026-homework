using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
	[BurstCompile, UpdateAfter(typeof(LifetimeSystem))]
	public partial struct ProjectileSystem : ISystem
	{
		private ComponentLookup<LocalTransform> _transformLookup;
		private ComponentLookup<Health> _healthLookup;
		private BufferLookup<TakeDamageRequest> _takeDamageRequests;

		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<GameState>();
			_transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
			_healthLookup = SystemAPI.GetComponentLookup<Health>();
			_takeDamageRequests = SystemAPI.GetBufferLookup<TakeDamageRequest>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (!SystemAPI.GetSingleton<GameState>().IsRunning)
				return;

			state.Dependency.Complete();
			_transformLookup.Update(ref state);
			_healthLookup.Update(ref state);
			_takeDamageRequests.Update(ref state);

			float deltaTime = SystemAPI.Time.DeltaTime;

			foreach ((
						 RefRW<LocalTransform> transform,
						 RefRO<MoveSpeed> moveSpeed,
						 RefRO<TargetEntity> targetRef,
						 RefRO<StoppingDistance> stoppingDistanceRef,
						 RefRO<Damage> damage,
						 RefRO<TargetOffset> offset,
						 RefRO<Instigator> instigator,
						 Entity entity
					 ) in SystemAPI.Query<
							 RefRW<LocalTransform>,
							 RefRO<MoveSpeed>,
							 RefRO<TargetEntity>,
							 RefRO<StoppingDistance>,
							 RefRO<Damage>,
							 RefRO<TargetOffset>,
							 RefRO<Instigator>>()
						 .WithAll<Projectile, Health>()
						 .WithEntityAccess())
			{
				Health health = _healthLookup[entity];

				if (health.IsDead())
					continue;

				Entity target = targetRef.ValueRO.value;

				if (target == Entity.Null || !_transformLookup.TryGetComponent(target, out LocalTransform targetTransform))
				{
					MoveUseCase.MoveStep(ref transform.ValueRW,
						transform.ValueRO.Forward(),
						in moveSpeed.ValueRO,
						deltaTime);

					continue;
				}

				float3 targetPosition = targetTransform.TransformPoint(offset.ValueRO.value);
				float3 delta = targetPosition - transform.ValueRO.Position;
				float stoppingDistance = stoppingDistanceRef.ValueRO.value;
				float distanceSq = math.lengthsq(delta);

				if (distanceSq > stoppingDistance * stoppingDistance)
				{
					float distance = math.sqrt(distanceSq);
					float remainingDistance = distance - stoppingDistance;
					float3 direction = delta / distance;
					float movedDistance = MoveUseCase.MoveStep(ref transform.ValueRW, direction, in moveSpeed.ValueRO,
						deltaTime, remainingDistance);
					transform.ValueRW.Rotation = ProjectileUseCase.GetRotation(direction);

					if (movedDistance < remainingDistance)
						continue;
				}

				if (_takeDamageRequests.TryGetBuffer(target, out DynamicBuffer<TakeDamageRequest> damageRequests))
					damageRequests.Add(new TakeDamageRequest
					{
						damage = damage.ValueRO.value,
						instigator = instigator.ValueRO.value
					});

				health.value = 0;
				_healthLookup[entity] = health;
			}
		}
	}
}
