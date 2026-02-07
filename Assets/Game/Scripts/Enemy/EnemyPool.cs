using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy
{
    public sealed class EnemyPool : MonoBehaviour
    {
        [SerializeField] private EnemyFactory _enemyFactory;
        
        private readonly Queue<EnemyFacade> _pool = new();
        
        public EnemyFacade Spawn(Vector3 spawnPosition)
        {
            if (_pool.TryDequeue(out EnemyFacade enemy))
                enemy.gameObject.SetActive(true);
            else
                enemy = _enemyFactory.Create(spawnPosition);

            return enemy;
        }
        
        public void Despawn(EnemyFacade enemy)
        {
            StartCoroutine(DespawnInNextFrame(enemy));
        }

        private IEnumerator DespawnInNextFrame(EnemyFacade enemy)
        {
            yield return null;
            enemy.gameObject.SetActive(false);
            _pool.Enqueue(enemy);
        }
    }
}
