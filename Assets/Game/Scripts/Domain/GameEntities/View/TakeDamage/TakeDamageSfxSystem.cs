using Modules.AudioEvents;
using Unity.Entities;
using Unity.Transforms;

namespace Game
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial struct TakeDamageSfxSystem : ISystem
	{
		private const float DamageThreshold = 0.1f;

		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<TakeDamageSfx>();
		}

		public void OnUpdate(ref SystemState state)
		{
			state.Dependency.Complete();

			AudioSystem audioSystem = AudioSystem.Instance;

			if (audioSystem == null)
				return;

			foreach ((RefRO<TakeDamageSfx> sfx, RefRO<LocalTransform> transform, DynamicBuffer<TakeDamageEvent> events)
					 in SystemAPI.Query<RefRO<TakeDamageSfx>, RefRO<LocalTransform>, DynamicBuffer<TakeDamageEvent>>())
			{
				if (sfx.ValueRO.value.Length == 0)
					continue;

				for (var i = 0; i < events.Length; i++)
				{
					if (events[i].damage <= 0)
						continue;

					var key = new AudioEventKey(sfx.ValueRO.value.ToString());

					audioSystem.PlayEvent(key,
						transform.ValueRO.Position,
						transform.ValueRO.Rotation,
						DamageThreshold);

					break;
				}
			}
		}
	}
}
