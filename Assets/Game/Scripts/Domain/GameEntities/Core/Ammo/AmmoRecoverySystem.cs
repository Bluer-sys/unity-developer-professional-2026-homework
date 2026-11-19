using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
	[BurstCompile]
	public partial struct AmmoRecoverySystem : ISystem
	{
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<GameState>();
			state.RequireForUpdate<AmmoRecovery>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (!SystemAPI.GetSingleton<GameState>().IsRunning)
				return;

			state.Dependency = new RecoveryJob
			{
				DeltaTime = SystemAPI.Time.DeltaTime
			}.ScheduleParallel(state.Dependency);
		}

		[BurstCompile]
		public partial struct RecoveryJob : IJobEntity
		{
			public float DeltaTime;

			private void Execute(ref Ammo ammo, ref AmmoRecovery recovery, in MaxAmmo maxAmmo, in Health health)
			{
				if (health.IsDead())
					return;

				if (ammo.value >= maxAmmo.value)
				{
					recovery.time = 0f;
					return;
				}

				recovery.time += DeltaTime;
				var count = (int)math.floor(recovery.time / recovery.duration);

				if (count <= 0)
					return;

				ammo.value = math.min(ammo.value + count, maxAmmo.value);
				recovery.time = ammo.value == maxAmmo.value ? 0f : recovery.time - count * recovery.duration;
			}
		}
	}
}
