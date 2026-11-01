using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
	[BurstCompile]
	public partial struct FireCooldownSystem : ISystem
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

			float deltaTime = SystemAPI.Time.DeltaTime;

			foreach (RefRW<FireCooldown> cooldown in SystemAPI.Query<RefRW<FireCooldown>>())
				cooldown.ValueRW.time = math.max(cooldown.ValueRW.time - deltaTime, 0);
		}
	}
}
