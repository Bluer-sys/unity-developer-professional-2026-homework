using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class RotationSpeedAuthoring : MonoBehaviour
	{
		[SerializeField]
		private RotationSpeed _value;

		private sealed class Baker : Baker<RotationSpeedAuthoring>
		{
			public override void Bake(RotationSpeedAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, authoring._value);
			}
		}
	}
}
