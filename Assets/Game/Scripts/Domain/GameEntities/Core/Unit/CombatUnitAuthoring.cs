using Modules.AudioEvents;
using Unity.Entities;
using UnityEngine;

namespace Game
{
	[DisallowMultipleComponent]
	public abstract class CombatUnitAuthoring : MonoBehaviour
	{
		[Header("Unit")]
		[SerializeField] private TeamType _team;
		[SerializeField] private int _health = 10;
		[SerializeField, Range(0f, 1f)] private float _armorMultiplier;

		[Header("Movement")]
		[SerializeField] private float _moveSpeed = 3f;
		[SerializeField] private float _rotationSpeed = 360f;

		[Header("Combat")]
		[SerializeField] private int _damage = 2;
		[SerializeField] private float _attackDistance = 1.5f;
		[SerializeField] private float _detectionRadius = 20f;
		[SerializeField] private float _detectionCooldown = 1f;
		[SerializeField] private float _fireCooldown = 1f;
		[SerializeField] private float _deathCooldown = 1f;

		[Header("Presentation")]
		[SerializeField] private TeamViewConfig _teamViewConfig;
		[SerializeField] private AudioEventSerialized _fireSfx;

		[BakeDerivedTypes]
		private sealed class Baker : Baker<CombatUnitAuthoring>
		{
			public override void Bake(CombatUnitAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);

				AddComponent<Unit>(entity);
				AddComponent(entity, new Team { value = authoring._team });
				AddComponent(entity, new Health { value = authoring._health });
				AddComponent(entity, new MaxHealth { value = authoring._health });
				AddComponent(entity, new ArmorMultiplier { value = authoring._armorMultiplier });
				AddComponent(entity, new MoveSpeed { value = authoring._moveSpeed });
				AddComponent(entity, new RotationSpeed { value = authoring._rotationSpeed });
				AddComponent(entity, new Damage { value = authoring._damage });
				AddComponent(entity, new AttackDistance { value = authoring._attackDistance });
				AddComponent(entity, new DetectionRadius { value = authoring._detectionRadius });
				AddComponent(entity, new TargetEntity { value = Entity.Null });
				AddComponent(entity, new EnemyBaseTarget { value = Entity.Null });

				AddComponent(entity, new DetectionCooldown
				{
					time = 0f,
					duration = authoring._detectionCooldown
				});

				AddComponent(entity, new FireCooldown
				{
					time = 0f,
					duration = authoring._fireCooldown
				});

				AddComponent(entity, new DeadCooldown
				{
					time = 0f,
					duration = authoring._deathCooldown
				});

				AddComponent<MoveRequest>(entity);
				SetComponentEnabled<MoveRequest>(entity, false);
				AddComponent<MoveEvent>(entity);
				SetComponentEnabled<MoveEvent>(entity, false);
				AddComponent<FireRequest>(entity);
				SetComponentEnabled<FireRequest>(entity, false);
				AddComponent<FireEvent>(entity);
				SetComponentEnabled<FireEvent>(entity, false);
				AddComponent<DeathEvent>(entity);
				SetComponentEnabled<DeathEvent>(entity, false);
				SetComponentEnabled<DeadCooldown>(entity, false);

				AddBuffer<TakeDamageRequest>(entity);
				AddBuffer<TakeDamageEvent>(entity);
				AddComponent(entity, new FireSfx { value = authoring._fireSfx });

				DependsOn(authoring._teamViewConfig);
				AddComponent(entity, new UnitMaterialBakingData
				{
					blue = authoring._teamViewConfig.GetTeam(TeamType.Blue).Material,
					red = authoring._teamViewConfig.GetTeam(TeamType.Red).Material
				});
				AddBuffer<UnitEquipment>(entity);
			}
		}
	}
}
