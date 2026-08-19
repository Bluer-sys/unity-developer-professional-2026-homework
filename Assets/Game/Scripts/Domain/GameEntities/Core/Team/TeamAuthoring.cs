using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class TeamAuthoring : MonoBehaviour
	{
		public TeamType Team => _teamType;

		[SerializeField]
		private TeamType _teamType;

		private sealed class Baker : Baker<TeamAuthoring>
		{
			public override void Bake(TeamAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);

				AddComponent(entity, new Team
				{
					value = authoring._teamType
				});
			}
		}
	}
}
