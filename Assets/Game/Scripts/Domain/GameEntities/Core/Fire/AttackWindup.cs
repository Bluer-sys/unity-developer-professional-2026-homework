using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct AttackWindup : IComponentData, IEnableableComponent
	{
		public float delay;
		public double readyTime;
		public Entity target;
	}
}
