using Unity.Entities;
using UnityEngine;

namespace Game
{
	public class DeadCooldownAuthoring : MonoBehaviour
	{
		[SerializeField]
		private float _duration;

		public class DeadCooldownBaker : Baker<DeadCooldownAuthoring>
		{
			public override void Bake(DeadCooldownAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);
				AddComponent(entity, new DeadCooldown { duration = authoring._duration });
				SetComponentEnabled<DeadCooldown>(entity, false);
			}
		}
	}
}
