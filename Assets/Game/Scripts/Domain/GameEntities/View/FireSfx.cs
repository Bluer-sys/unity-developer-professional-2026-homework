using System;
using Modules.AudioEvents;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct FireSfx : IComponentData
	{
		public AudioEventKey value;
	}
}
