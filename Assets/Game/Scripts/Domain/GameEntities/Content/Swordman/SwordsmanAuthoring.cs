using Unity.Entities;

namespace Game
{
	public sealed class SwordsmanAuthoring : CombatUnitAuthoring
	{
		private sealed class Baker : Baker<SwordsmanAuthoring>
		{
			public override void Bake(SwordsmanAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent<Swordsman>(entity);
			}
		}
	}
}
