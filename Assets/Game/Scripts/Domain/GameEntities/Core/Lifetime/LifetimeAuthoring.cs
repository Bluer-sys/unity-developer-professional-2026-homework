using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class LifetimeAuthoring : MonoBehaviour
	{
		[SerializeField]
		private float _value;

		private sealed class Baker : Baker<LifetimeAuthoring>
		{
			public override void Bake(LifetimeAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);

				AddComponent(entity, new Lifetime
				{
					value = authoring._value
				});
			}
		}
	}
}
