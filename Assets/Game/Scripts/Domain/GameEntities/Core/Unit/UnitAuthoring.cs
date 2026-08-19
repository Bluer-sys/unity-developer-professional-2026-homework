using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class UnitAuthoring : MonoBehaviour
	{
		private sealed class Baker : Baker<UnitAuthoring>
		{
			public override void Bake(UnitAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);
				AddComponent<Unit>(entity);
			}
		}
	}
}
