using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct MoneyIncome : IComponentData
	{
		public float time;
		public float duration;
	}
}
