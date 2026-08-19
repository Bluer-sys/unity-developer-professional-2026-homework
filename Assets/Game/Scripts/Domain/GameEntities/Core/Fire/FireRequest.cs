using System;
using Unity.Entities;

namespace Game
{
	// FireRequest - one frame — single time

	[Serializable]
	public struct FireRequest : IComponentData, IEnableableComponent
	{
		public Entity target;
	}
}
