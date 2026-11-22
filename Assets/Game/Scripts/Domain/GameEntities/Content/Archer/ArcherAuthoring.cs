using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class ArcherAuthoring : CombatUnitAuthoring
	{
		[Header("Archer")]
		[SerializeField] private int _ammo = 5;
		[SerializeField] private int _maxAmmo = 5;
		[SerializeField] private float _ammoRecoveryDuration = 2f;
		[SerializeField] private GameObject _projectilePrefab;
		[SerializeField] private Vector3 _fireOffset = new(0f, 1f, 0.5f);

		private sealed class Baker : Baker<ArcherAuthoring>
		{
			public override void Bake(ArcherAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent<Archer>(entity);
				AddComponent(entity, new Ammo { value = authoring._ammo });
				AddComponent(entity, new MaxAmmo { value = authoring._maxAmmo });
				AddComponent(entity, new AmmoRecovery
				{
					time = 0f,
					duration = authoring._ammoRecoveryDuration
				});
				AddComponent(entity, new FireOffset { value = authoring._fireOffset });
				AddComponent(entity, new ProjectilePrefab
				{
					value = GetEntity(authoring._projectilePrefab, TransformUsageFlags.Dynamic)
				});
			}
		}
	}
}
