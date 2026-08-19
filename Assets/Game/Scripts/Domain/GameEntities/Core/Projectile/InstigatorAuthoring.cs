using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class InstigatorAuthoring : MonoBehaviour
	{
		private sealed class Baker : Baker<InstigatorAuthoring>
		{
			public override void Bake(InstigatorAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);
				AddComponent(entity, new Instigator { value = Entity.Null });
			}
		}
	}
}
