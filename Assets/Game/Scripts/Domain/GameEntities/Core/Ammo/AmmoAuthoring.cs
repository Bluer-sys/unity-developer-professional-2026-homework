using Unity.Entities;
using UnityEngine;

namespace Game
{
	public class AmmoAuthoring : MonoBehaviour
	{
		[SerializeField]
		private int _value = 5;

		[SerializeField]
		private int _maxValue = 5;

		[SerializeField]
		private float _recoveryDuration = 2f;

		public class AmmoBaker : Baker<AmmoAuthoring>
		{
			public override void Bake(AmmoAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new Ammo { value = authoring._value });
				AddComponent(entity, new MaxAmmo { value = authoring._maxValue });
				AddComponent(entity, new AmmoRecovery
				{
					time = 0f,
					duration = authoring._recoveryDuration
				});
			}
		}
	}
}
