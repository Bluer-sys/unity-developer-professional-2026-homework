using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct EnemyBaseTarget : IComponentData
	{
		public Entity value;
	}
}
