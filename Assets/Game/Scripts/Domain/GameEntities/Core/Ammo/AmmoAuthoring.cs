using Unity.Entities;
using UnityEngine;

namespace Game
{
	public class AmmoAuthoring : MonoBehaviour
	{
		public int Value;

		public class AmmoBaker : Baker<AmmoAuthoring>
		{
			public override void Bake(AmmoAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new Ammo { value = authoring.Value });
			}
		}
	}
}
