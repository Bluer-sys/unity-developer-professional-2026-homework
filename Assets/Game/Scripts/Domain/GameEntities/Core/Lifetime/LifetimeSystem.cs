using Unity.Burst;
using Unity.Entities;

namespace Game
{
	[BurstCompile]
	public partial struct LifetimeSystem : ISystem
	{
		private ComponentLookup<Health> _healthLookup;

		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
			_healthLookup = SystemAPI.GetComponentLookup<Health>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			state.Dependency.Complete();
			_healthLookup.Update(ref state);

			float deltaTime = SystemAPI.Time.DeltaTime;

			EntityCommandBuffer ecb = SystemAPI
				.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
				.CreateCommandBuffer(state.WorldUnmanaged);

			foreach ((RefRW<Lifetime> lifetime, Entity entity) in SystemAPI.Query<RefRW<Lifetime>>().WithEntityAccess())
			{
				ref float time = ref lifetime.ValueRW.value;
				time -= deltaTime;

				if (time > 0f)
					continue;

				if (_healthLookup.TryGetComponent(entity, out Health health))
				{
					health.value = 0;
					_healthLookup[entity] = health;
				}
				else
				{
					ecb.DestroyEntity(entity);
				}
			}
		}
	}
}
