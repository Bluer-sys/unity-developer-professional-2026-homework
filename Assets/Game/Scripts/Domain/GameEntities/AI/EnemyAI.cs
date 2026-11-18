using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct EnemyAI : IComponentData
	{
		public int unitIndex;
		public bool hasSelectedUnit;
		public bool isWaitingForPurchase;
	}
}
