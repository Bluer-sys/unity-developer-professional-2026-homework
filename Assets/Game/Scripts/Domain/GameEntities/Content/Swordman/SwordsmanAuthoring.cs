using Unity.Entities;
using UnityEngine;

namespace Game
{
	[RequireComponent(typeof(UnitAuthoring)), RequireComponent(typeof(MoveRequestAuthoring)), RequireComponent(typeof(MoveEventAuthoring)), RequireComponent(typeof(HealthAuthoring))]
	public sealed class SwordsmanAuthoring : MonoBehaviour
	{
		private sealed class Baker : Baker<SwordsmanAuthoring>
		{
			public override void Bake(SwordsmanAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent<Swordsman>(entity);
			}
		}
	}
}
