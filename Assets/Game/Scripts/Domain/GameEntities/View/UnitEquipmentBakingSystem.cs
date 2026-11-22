using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

namespace Game
{
	[WorldSystemFilter(WorldSystemFilterFlags.BakingSystem)]
	[UpdateInGroup(typeof(PostBakingSystemGroup))]
	public partial struct UnitEquipmentBakingSystem : ISystem
	{
		public void OnUpdate(ref SystemState state)
		{
			foreach (DynamicBuffer<UnitEquipment> equipment in SystemAPI.Query<DynamicBuffer<UnitEquipment>>()
						 .WithOptions(EntityQueryOptions.IncludePrefab | EntityQueryOptions.IncludeDisabledEntities))
				equipment.Clear();

			var animations = SystemAPI.GetComponentLookup<UnitAnimation>(true);
			var unitMaterials = SystemAPI.GetComponentLookup<UnitMaterialBakingData>(true);
			var equipmentBuffers = SystemAPI.GetBufferLookup<UnitEquipment>();
			using var ecb = new EntityCommandBuffer(Allocator.Temp);

			foreach (var (lod, meshInfo, entity) in SystemAPI.Query<RefRO<MeshLODComponent>, RefRO<MaterialMeshInfo>>()
						 .WithAll<RenderMeshArray>()
						 .WithOptions(EntityQueryOptions.IncludePrefab | EntityQueryOptions.IncludeDisabledEntities |
							 EntityQueryOptions.IgnoreComponentEnabledState)
						 .WithEntityAccess())
			{
				if (!animations.TryGetComponent(lod.ValueRO.Group, out UnitAnimation animation))
					continue;

				UnitMaterialBakingData palette = unitMaterials[animation.unit];
				var renderMeshes = state.EntityManager.GetSharedComponentManaged<RenderMeshArray>(entity);
				if (renderMeshes.GetMaterial(meshInfo.ValueRO) != palette.blue.Value)
					continue;

				var materials = renderMeshes.MaterialReferences;
				var part = new UnitEquipment
				{
					entity = entity,
					blueMaterial = meshInfo.ValueRO.Material,
					redMaterial = GetMaterialIndex(ref materials, palette.red)
				};
				equipmentBuffers[animation.unit].Add(part);

				if (materials != renderMeshes.MaterialReferences)
				{
					renderMeshes.MaterialReferences = materials;
					renderMeshes.ResetHash128();
					ecb.SetSharedComponentManaged(entity, renderMeshes);
				}
			}

			ecb.Playback(state.EntityManager);
		}

		private static int GetMaterialIndex(ref UnityObjectRef<Material>[] materials, UnityObjectRef<Material> material)
		{
			int index = Array.IndexOf(materials, material);
			if (index < 0)
			{
				index = materials.Length;
				Array.Resize(ref materials, index + 1);
				materials[index] = material;
			}

			return MaterialMeshInfo.ArrayIndexToStaticIndex(index);
		}
	}
}
