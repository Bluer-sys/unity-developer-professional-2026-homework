using Modules.AudioEvents;
using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class FireSfxAuthoring : MonoBehaviour
	{
		[SerializeField]
		private AudioEventSerialized _value;

		private sealed class Baker : Baker<FireSfxAuthoring>
		{
			public override void Bake(FireSfxAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new FireSfx { value = authoring._value });
			}
		}
	}
}
