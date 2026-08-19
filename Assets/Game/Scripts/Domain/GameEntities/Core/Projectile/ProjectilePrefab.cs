using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct ProjectilePrefab : IComponentData
	{
		public Entity value;
	}
}
