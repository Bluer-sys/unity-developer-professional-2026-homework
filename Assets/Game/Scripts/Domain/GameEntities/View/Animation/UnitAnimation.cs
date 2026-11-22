using Unity.Entities;

namespace Game
{
	public struct UnitAnimation : IComponentData
	{
		public Entity unit;
		public byte isMoving;
		public byte fire;
		public byte takeDamage;
		public byte death;
	}
}
