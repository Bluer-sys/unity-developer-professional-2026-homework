using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class FireCooldownAuthoring : MonoBehaviour
	{
		[SerializeField]
		private FireCooldown _cooldown;

		private sealed class Baker : Baker<FireCooldownAuthoring>
		{
			public override void Bake(FireCooldownAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);
				AddComponent(entity, authoring._cooldown);
			}
		}
	}
}
