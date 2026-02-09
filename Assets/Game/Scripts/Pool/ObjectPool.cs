using System;
using System.Collections;
using System.Collections.Generic;
using Game.Factory;
using UnityEngine;

namespace Game.Pool
{
    public abstract class ObjectPool<TObject, TFactory> : MonoBehaviour
        where TObject : Component
        where TFactory : IFactory<TObject>
    {
        [SerializeField] private int _initialSize;
        [SerializeField] private TFactory _factory;

        private readonly Stack<TObject> _pool = new();

        private void Awake()
        {
            for (var i = 0; i < _initialSize; i++)
            {
                TObject obj = _factory.Create(Vector3.zero, Quaternion.identity);
                obj.gameObject.SetActive(false);
                _pool.Push(obj);
            }
        }

        public TObject Spawn(Vector3 position, Quaternion rotation, Transform parent)
        {
            if (_pool.TryPop(out TObject obj))
            {
                obj.gameObject.SetActive(true);
                obj.transform.parent = parent;
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                Reinitialize(obj);
            }
            else
            {
                obj = _factory.Create(position, rotation);
            }

            return obj;
        }

        public void Despawn(TObject obj)
        {
            StartCoroutine(DespawnInNextFrame(obj));
        }

        protected abstract void Reinitialize(TObject obj);
        
        private IEnumerator DespawnInNextFrame(TObject enemy)
        {
            yield return null;

            enemy.gameObject.SetActive(false);
            _pool.Push(enemy);
        }
    }
}
