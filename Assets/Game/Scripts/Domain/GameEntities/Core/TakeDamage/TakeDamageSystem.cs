using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
	[BurstCompile, UpdateAfter(typeof(SwordsmanMeleeFireSystem)), UpdateAfter(typeof(ProjectileSystem))]
	public partial struct TakeDamageSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			foreach ((
						 RefRW<Health> health,
						 DynamicBuffer<TakeDamageRequest> requests,
						 DynamicBuffer<TakeDamageEvent> events,
						 RefRO<ArmorMultiplier> armor
					 ) in SystemAPI.Query<
						 RefRW<Health>,
						 DynamicBuffer<TakeDamageRequest>,
						 DynamicBuffer<TakeDamageEvent>,
						 RefRO<ArmorMultiplier>>())
			{
				for (var i = 0; i < requests.Length && health.ValueRO.IsAlive(); i++)
				{
					TakeDamageRequest request = requests[i];
					var damage = (int)math.round(request.damage * (1f - armor.ValueRO.value));

					health.ValueRW.Reduce(damage);

					events.Add(new TakeDamageEvent
					{
						damage = damage,
						instigator = request.instigator
					});
				}

				requests.Clear();
			}
		}
	}
}
