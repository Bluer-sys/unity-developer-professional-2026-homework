using Unity.Burst;
using Unity.Entities;

namespace Game
{
	[BurstCompile, UpdateAfter(typeof(AttackWindupSystem))]
	public partial struct SwordsmanMeleeFireSystem : ISystem
	{
		private BufferLookup<TakeDamageRequest> _takeDamageRequests;

		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<GameState>();
			_takeDamageRequests = SystemAPI.GetBufferLookup<TakeDamageRequest>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (!SystemAPI.GetSingleton<GameState>().IsRunning)
				return;

			_takeDamageRequests.Update(ref state);

			foreach (var (request, damage, entity) in
					 SystemAPI.Query<RefRO<AttackReadyRequest>, RefRO<Damage>>()
						 .WithAll<Swordsman>()
						 .WithEntityAccess())
			{
				Entity target = request.ValueRO.target;
				SystemAPI.SetComponentEnabled<AttackReadyRequest>(entity, false);

				if (!_takeDamageRequests.TryGetBuffer(target, out DynamicBuffer<TakeDamageRequest> requests))
					continue;

				requests.Add(new TakeDamageRequest
				{
					damage = damage.ValueRO.value,
					instigator = entity
				});
				SystemAPI.SetComponentEnabled<FireEvent>(entity, true);
			}
		}
	}
}
