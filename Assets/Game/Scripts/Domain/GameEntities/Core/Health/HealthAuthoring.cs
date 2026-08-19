using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class HealthAuthoring : MonoBehaviour
	{
		[SerializeField]
		private int _current = 10;

		[SerializeField]
		private int _max = 10;

		private sealed class Baker : Baker<HealthAuthoring>
		{
			public override void Bake(HealthAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);

				AddComponent(entity, new Health
				{
					value = authoring._current
				});

				AddComponent(entity, new MaxHealth
				{
					value = authoring._max
				});
			}
		}
	}
}
