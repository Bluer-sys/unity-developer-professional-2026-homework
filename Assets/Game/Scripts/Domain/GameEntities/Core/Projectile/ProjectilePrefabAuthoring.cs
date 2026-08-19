using Unity.Entities;
using UnityEngine;

namespace Game
{
	[DisallowMultipleComponent]
	public sealed class ProjectilePrefabAuthoring : MonoBehaviour
	{
		[SerializeField]
		private GameObject _prefab;

		private sealed class Baker : Baker<ProjectilePrefabAuthoring>
		{
			public override void Bake(ProjectilePrefabAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);

				AddComponent(entity, new ProjectilePrefab
				{
					value = GetEntity(authoring._prefab, TransformUsageFlags.Dynamic)
				});
			}
		}
	}
}
