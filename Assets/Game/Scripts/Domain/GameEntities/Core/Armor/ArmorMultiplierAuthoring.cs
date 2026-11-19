using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class ArmorMultiplierAuthoring : MonoBehaviour
	{
		[SerializeField]
		private float _value;

		public class ArmorMultiplierBaker : Baker<ArmorMultiplierAuthoring>
		{
			public override void Bake(ArmorMultiplierAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new ArmorMultiplier { value = authoring._value });
			}
		}
	}
}
