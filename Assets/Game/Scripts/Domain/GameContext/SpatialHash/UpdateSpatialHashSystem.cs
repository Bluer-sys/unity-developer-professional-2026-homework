using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Transforms;

namespace Game
{
	[BurstCompile]
	public unsafe partial struct UpdateSpatialHashSystem : ISystem
	{
		private EntityQuery _unitQuery;

		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<SpatialHashData>();

			_unitQuery = SystemAPI.QueryBuilder()
				.WithAll<Unit, LocalTransform>()
				.Build();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			state.Dependency.Complete();

			var spatialHashData = SystemAPI.GetSingleton<SpatialHashData>();
			UnsafeParallelMultiHashMap<uint, Entity>* spatialHash = spatialHashData.map;

			int unitCount = _unitQuery.CalculateEntityCount();

			if (unitCount > spatialHash->Capacity)
				spatialHash->Capacity = unitCount;

			spatialHash->Clear();

			state.Dependency = new UpdateJob(*spatialHash, spatialHashData.cellSize)
				.ScheduleParallel(state.Dependency);

			state.Dependency.Complete();
		}

		[WithAll(typeof(Unit)), BurstCompile]
		private partial struct UpdateJob : IJobEntity
		{
			private UnsafeParallelMultiHashMap<uint, Entity>.ParallelWriter _spatialHash;
			private readonly float _cellSize;

			public UpdateJob(UnsafeParallelMultiHashMap<uint, Entity> spatialHash, float cellSize) : this()
			{
				_spatialHash = spatialHash.AsParallelWriter();
				_cellSize = cellSize;
			}

			private void Execute(Entity entity, in LocalTransform transform)
			{
				uint hash = SpatialHashUseCase.Hash(transform.Position, _cellSize);
				_spatialHash.Add(hash, entity);
			}
		}
	}
}
