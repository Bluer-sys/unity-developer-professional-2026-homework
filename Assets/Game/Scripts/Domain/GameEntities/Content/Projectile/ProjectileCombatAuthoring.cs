using Unity.Entities;
using UnityEngine;

namespace Game
{
	[DisallowMultipleComponent]
	public sealed class ProjectileCombatAuthoring : MonoBehaviour
	{
		[SerializeField] private int _health = 1;
		[SerializeField] private float _moveSpeed = 20f;
		[SerializeField] private float _stoppingDistance = 0.35f;
		[SerializeField] private int _damage = 2;
		[SerializeField] private float _lifetime = 3f;
		[SerializeField] private Vector3 _targetOffset = new(0f, 1f, 0f);

		private sealed class Baker : Baker<ProjectileCombatAuthoring>
		{
			public override void Bake(ProjectileCombatAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);

				AddComponent<Projectile>(entity);
				AddComponent(entity, new Health { value = authoring._health });
				AddComponent(entity, new MaxHealth { value = authoring._health });
				AddComponent<ArmorMultiplier>(entity);
				AddBuffer<TakeDamageRequest>(entity);
				AddBuffer<TakeDamageEvent>(entity);
				AddComponent<DeadCooldown>(entity);
				SetComponentEnabled<DeadCooldown>(entity, false);
				AddComponent<DeathEvent>(entity);
				SetComponentEnabled<DeathEvent>(entity, false);
				AddComponent(entity, new Team { value = TeamType.Neutral });
				AddComponent(entity, new TargetEntity { value = Entity.Null });
				AddComponent(entity, new Instigator { value = Entity.Null });
				AddComponent(entity, new MoveSpeed { value = authoring._moveSpeed });
				AddComponent(entity, new StoppingDistance { value = authoring._stoppingDistance });
				AddComponent(entity, new Damage { value = authoring._damage });
				AddComponent(entity, new Lifetime { value = authoring._lifetime });
				AddComponent(entity, new TargetOffset { value = authoring._targetOffset });
			}
		}
	}
}
