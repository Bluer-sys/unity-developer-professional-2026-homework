using Unity.Entities;
using UnityEngine;

namespace Game
{
	public class DeathEventAuthoring : MonoBehaviour
	{
		public class DeathEventBaker : Baker<DeathEventAuthoring>
		{
			public override void Bake(DeathEventAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent<DeathEvent>(entity);
				SetComponentEnabled<DeathEvent>(entity, false);
			}
		}
	}
}
