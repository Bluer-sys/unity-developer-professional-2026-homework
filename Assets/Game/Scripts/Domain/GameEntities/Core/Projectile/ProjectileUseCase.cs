using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
	public static class ProjectileUseCase
	{
		public static quaternion GetRotation(float3 direction)
		{
			float3 up = math.abs(direction.y) < 0.999f ? math.up() : math.forward();
			return quaternion.LookRotationSafe(direction, up);
		}

		public static void SpawnProjectile(
				ref EntityCommandBuffer ecb,
				ProjectilePrefab projectilePrefab,
				LocalTransform transform,
				Team team,
				Entity target,
				Entity instigator
			)
		{
			Entity projectile = ecb.Instantiate(projectilePrefab.value);

			ecb.SetComponent(projectile, transform);
			ecb.SetComponent(projectile, team);

			ecb.SetComponent(projectile, new TargetEntity
			{
				value = target
			});

			ecb.SetComponent(projectile, new Instigator
			{
				value = instigator
			});
		}
	}
}
