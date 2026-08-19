using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class FireRequestAuthoring : MonoBehaviour
	{
		private sealed class Baker : Baker<FireRequestAuthoring>
		{
			public override void Bake(FireRequestAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);
				AddComponent<FireRequest>(entity);
				SetComponentEnabled<FireRequest>(entity, false);
			}
		}
	}
}
