using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class MoveSpeedAuthoring : MonoBehaviour
	{
		[SerializeField]
		private MoveSpeed _value;

		private sealed class Baker : Baker<MoveSpeedAuthoring>
		{
			public override void Bake(MoveSpeedAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, authoring._value);
			}
		}
	}
}
