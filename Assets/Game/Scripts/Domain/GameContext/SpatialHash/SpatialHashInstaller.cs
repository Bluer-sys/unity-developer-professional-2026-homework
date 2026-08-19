using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
	[Serializable]
	public unsafe struct SpatialHashInstaller // MonoInstaller (Zenject)
	{
		[SerializeField]
		private int _initialCapacity; //2048

		[SerializeField]
		private int _cellSize; // 1

		private SpatialHashData _spatialHash;

		public void Install(Entity gameContext, EntityManager entityManager)
		{
			int capacity = math.max(_initialCapacity, 1);
			float cellSize = math.max(_cellSize, 1);
			var hash = new UnsafeParallelMultiHashMap<uint, Entity>(capacity, Allocator.Persistent);

			_spatialHash = new SpatialHashData
			{
				map = UnsafeUseCase.AllocPointer(hash, Allocator.Persistent),
				cellSize = cellSize
			};

			entityManager.AddComponentData(gameContext, _spatialHash);
		}

		public void Uninstall()
		{
			if (_spatialHash.map != null)
			{
				_spatialHash.map->Dispose();
				UnsafeUseCase.FreePointer(_spatialHash.map, Allocator.Persistent);
			}
		}
	}
}
