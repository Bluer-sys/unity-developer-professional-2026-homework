using Unity.Entities;
using UnityEngine;

namespace Game
{
	[BakingType]
	public struct UnitMaterialBakingData : IComponentData
	{
		public UnityObjectRef<Material> blue;
		public UnityObjectRef<Material> red;
	}
}
