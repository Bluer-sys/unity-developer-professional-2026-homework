using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
	public static class FireUseCase
	{
		public static bool IsExpired(in this FireCooldown cooldown)
		{
			return cooldown.time <= 0;
		}

		public static bool IsPlaying(in this FireCooldown cooldown)
		{
			return cooldown.time > 0;
		}

		public static void ResetTime(ref this FireCooldown cooldown)
		{
			cooldown.time = cooldown.duration;
		}

		public static float3 GetFirePoint(in LocalTransform transform, in FireOffset fireOffset)
		{
			return transform.TransformPoint(fireOffset.value);
		}

		public static bool TryAimAtTarget(
				Entity attacker,
				Entity target,
				in Team team,
				in AttackDistance range,
				ref LocalTransform transform,
				in ComponentLookup<Team> teams,
				in ComponentLookup<Health> healths,
				in ComponentLookup<LocalTransform> transforms,
				in ComponentLookup<ProjectilePrefab> projectilePrefabs
			)
		{
			var isEnemy = new IsEnemyPredicate(attacker, team.value, teams, healths);
			if (!isEnemy.Invoke(target) || !transforms.TryGetComponent(target, out LocalTransform targetTransform))
				return false;

			float3 delta = targetTransform.Position - transform.Position;
			if (math.lengthsq(delta) > range.value * range.value)
				return false;

			if (projectilePrefabs.HasComponent(attacker))
				delta.y = 0f;

			if (math.lengthsq(delta) > 0f)
				transform.Rotation = quaternion.LookRotationSafe(math.normalize(delta), math.up());

			return true;
		}
	}
}
