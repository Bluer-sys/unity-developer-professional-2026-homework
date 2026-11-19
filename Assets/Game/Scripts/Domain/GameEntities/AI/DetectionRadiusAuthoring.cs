using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class DetectionRadiusAuthoring : MonoBehaviour
	{
		[SerializeField]
		private float _value;

		[SerializeField]
		private float _cooldown = 1f;

		private sealed class Baker : Baker<DetectionRadiusAuthoring>
		{
			public override void Bake(DetectionRadiusAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);

				AddComponent(entity, new DetectionRadius
				{
					value = authoring._value
				});
				AddComponent(entity, new DetectionCooldown
				{
					time = 0f,
					duration = authoring._cooldown
				});
			}
		}
	}
}
