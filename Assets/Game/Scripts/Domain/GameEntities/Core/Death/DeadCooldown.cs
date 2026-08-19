using Unity.Entities;

namespace Game
{
	public struct DeadCooldown : IComponentData, IEnableableComponent
	{
		public float time;
		public float duration;
	}
}
