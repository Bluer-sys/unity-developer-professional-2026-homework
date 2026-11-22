using System;
using SnivelerCode.GpuAnimation.Runtime.Authoring;
using Unity.Entities;

namespace Game
{
	public sealed class UnitAnimationBaker : Baker<AnimatorAuthoring>
	{
		private const int TriggerMaskBits = 32;

		public override void Bake(AnimatorAuthoring authoring)
		{
			CombatUnitAuthoring unit = GetComponentInParent<CombatUnitAuthoring>();
			if (unit == null)
				return;

			AddComponent(GetEntity(TransformUsageFlags.Dynamic), new UnitAnimation
			{
				unit = GetEntity(unit, TransformUsageFlags.None),
				isMoving = GetParameter(authoring, "IsMoving", false),
				fire = GetParameter(authoring, "Fire", true),
				takeDamage = GetParameter(authoring, "TakeDamage", true),
				death = GetParameter(authoring, "Death", true)
			});
		}

		private static byte GetParameter(AnimatorAuthoring authoring, string name, bool isTrigger)
		{
			int index = authoring.Parameters.FindIndex(parameter =>
				parameter.Name == name && parameter.IsTrigger == isTrigger);
			if (index < 0 || (isTrigger && index >= TriggerMaskBits))
				throw new InvalidOperationException(
					$"{authoring.name} has no usable GPU animation parameter '{name}' (trigger = {isTrigger}).");

			return checked((byte)index);
		}
	}
}
