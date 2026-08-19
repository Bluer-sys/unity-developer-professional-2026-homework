using Unity.Entities;
using UnityEngine;

namespace Game
{
	[RequireComponent(typeof(UnitAuthoring)), RequireComponent(typeof(MoveRequestAuthoring)), RequireComponent(typeof(MoveEventAuthoring)), RequireComponent(typeof(HealthAuthoring))]
	public sealed class ArcherAuthoring : MonoBehaviour
	{
		private sealed class Baker : Baker<ArcherAuthoring>
		{
			public override void Bake(ArcherAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent<Archer>(entity);
			}
		}
	}
}
