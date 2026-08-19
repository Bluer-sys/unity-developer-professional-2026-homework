using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct Team : IComponentData
	{
		public TeamType value;
	}
}
