using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
	public sealed class TargetOffsetAuthoring : MonoBehaviour
	{
		public float3 Value;

		public class TargetOffsetBaker : Baker<TargetOffsetAuthoring>
		{
			public override void Bake(TargetOffsetAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new TargetOffset { value = authoring.Value });
			}
		}
	}
}
