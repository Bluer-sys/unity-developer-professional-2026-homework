using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class FireEventAuthoring : MonoBehaviour
	{
		private sealed class Baker : Baker<FireEventAuthoring>
		{
			public override void Bake(FireEventAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);
				AddComponent<FireEvent>(entity);
				SetComponentEnabled<FireEvent>(entity, false);
			}
		}
	}
}
