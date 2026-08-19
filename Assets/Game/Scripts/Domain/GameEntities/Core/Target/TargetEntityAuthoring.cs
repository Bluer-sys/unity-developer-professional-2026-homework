using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class TargetEntityAuthoring : MonoBehaviour
	{
		[SerializeField]
		private GameObject _target;

		private sealed class Baker : Baker<TargetEntityAuthoring>
		{
			public override void Bake(TargetEntityAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);

				AddComponent(entity, new TargetEntity
				{
					value = GetEntity(authoring._target, TransformUsageFlags.None)
				});
			}
		}
	}
}
