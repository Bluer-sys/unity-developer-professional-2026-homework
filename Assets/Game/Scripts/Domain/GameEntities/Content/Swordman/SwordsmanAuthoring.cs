using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class SwordsmanAuthoring : CombatUnitAuthoring
	{
		[Header("Swordsman")]
		[SerializeField] private float _meleeHitDelay = 0.4f;

		private sealed class Baker : Baker<SwordsmanAuthoring>
		{
			public override void Bake(SwordsmanAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent<Swordsman>(entity);
				AddComponent(entity, new AttackWindup { delay = authoring._meleeHitDelay });
				SetComponentEnabled<AttackWindup>(entity, false);
			}
		}
	}
}
