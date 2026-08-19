using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class StoppingDistanceAuthoring : MonoBehaviour
	{
		[SerializeField]
		private float _value;

		private sealed class Baker : Baker<StoppingDistanceAuthoring>
		{
			public override void Bake(StoppingDistanceAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);

				AddComponent(entity, new StoppingDistance
				{
					value = authoring._value
				});
			}
		}
	}
}
