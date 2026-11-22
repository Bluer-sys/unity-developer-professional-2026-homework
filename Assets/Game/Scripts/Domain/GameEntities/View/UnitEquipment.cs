using Unity.Entities;

namespace Game
{
	public struct UnitEquipment : IBufferElementData
	{
		public Entity entity;
		public int blueMaterial;
		public int redMaterial;
	}
}
