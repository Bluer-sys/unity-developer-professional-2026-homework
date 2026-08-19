using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class DamageAuthoring : MonoBehaviour
	{
		[SerializeField]
		private int _damage;

		private sealed class Baker : Baker<DamageAuthoring>
		{
			public override void Bake(DamageAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);

				AddComponent(entity, new Damage
				{
					value = authoring._damage
				});
			}
		}
	}
}
