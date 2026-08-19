using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class GameContextInstaller : MonoBehaviour
	{
		private World _world;
		private EntityManager _entityManager;
		private Entity _entity;

		[SerializeField]
		private SpatialHashInstaller _spatialHashInstaller;

		// Inventory Installer

		// Camera Installer

		// Another installer

		private void Awake()
		{
			_world = World.DefaultGameObjectInjectionWorld;

			if (_world == null || !_world.IsCreated)
				return;

			_entityManager = _world.EntityManager;
			_entity = _entityManager.CreateEntity();
			_entityManager.SetName(_entity, "GameContext");

			_spatialHashInstaller.Install(_entity, _entityManager);
		}

		private void OnDestroy()
		{
			if (_world != null && _world.IsCreated)
				_entityManager.CompleteAllTrackedJobs();

			_spatialHashInstaller.Uninstall();

			if (_world != null && _world.IsCreated && _entityManager.Exists(_entity))
				_entityManager.DestroyEntity(_entity);
		}
	}
}
