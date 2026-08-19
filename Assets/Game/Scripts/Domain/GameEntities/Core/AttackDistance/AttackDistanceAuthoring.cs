using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class AttackDistanceAuthoring : MonoBehaviour
	{
		[SerializeField]
		private float _value;

		private sealed class Baker : Baker<AttackDistanceAuthoring>
		{
			public override void Bake(AttackDistanceAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);

				AddComponent(entity, new AttackDistance
				{
					value = authoring._value
				});
			}
		}
	}
}
