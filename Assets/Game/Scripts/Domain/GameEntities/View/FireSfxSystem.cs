using Modules.AudioEvents;
using Unity.Entities;
using Unity.Transforms;

namespace Game
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial struct FireSfxSystem : ISystem
	{
		private const float FIRE_THRESHOLD = 0.1f;

		public void OnUpdate(ref SystemState state)
		{
			state.Dependency.Complete();

			AudioSystem audioSystem = AudioSystem.Instance;

			if (audioSystem == null)
				return;

			foreach ((RefRO<FireSfx> sfx, RefRO<LocalTransform> transform)
					 in SystemAPI.Query<RefRO<FireSfx>, RefRO<LocalTransform>>()
						 .WithAll<FireEvent>())
				audioSystem.PlayEvent(sfx.ValueRO.value,
					transform.ValueRO.Position,
					transform.ValueRO.Rotation,
					FIRE_THRESHOLD);
		}
	}
}
