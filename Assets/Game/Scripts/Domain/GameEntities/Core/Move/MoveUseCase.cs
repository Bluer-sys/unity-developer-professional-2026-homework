using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
	public static class MoveUseCase
	{
		public static float MoveStep(
				ref LocalTransform transform,
				in float3 direction,
				in MoveSpeed speed,
				float deltaTime,
				float maxDistance = float.MaxValue
			)
		{
			if (math.all(direction == float3.zero))
				return 0f;

			float distance = math.min(speed.value * deltaTime, maxDistance);
			transform.Position += direction * distance;
			return distance;
		}
	}
}
