using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class TakeDamageSfxAuthoring : MonoBehaviour
	{
		[SerializeField]
		private string _value;

		private sealed class Baker : Baker<TakeDamageSfxAuthoring>
		{
			public override void Bake(TakeDamageSfxAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new TakeDamageSfx { value = authoring._value });
			}
		}
	}
}
