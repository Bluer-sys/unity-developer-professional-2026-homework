using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameContext.Core
{
    public abstract class ObjectPool<TObject, TFactory> : MonoBehaviour
        where TObject : Component
        where TFactory : IFactory<TObject>
    {
        [SerializeField] private int _initialSize;
        [SerializeField] private TFactory _factory;
        [SerializeField] private Transform _container;

        private readonly Stack<TObject> _pool = new();

        private void Awake()
        {
            for (var i = 0; i < _initialSize; i++)
            {
                TObject obj = Create(Vector3.zero, Quaternion.identity);
                obj.gameObject.SetActive(false);
                _pool.Push(obj);
            }
        }

        public TObject Spawn(Vector3 position, Quaternion rotation)
        {
            if (_pool.TryPop(out TObject obj))
            {
                obj.transform.SetPositionAndRotation(position, rotation);
            }
            else
            {
                obj = Create(position, rotation);
            }
            
            obj.gameObject.SetActive(true);

            Reinitialize(obj);
            
            return obj;
        }

        public void Despawn(TObject obj)
        {
            StartCoroutine(DespawnInNextFrame(obj));
        }

        private IEnumerator DespawnInNextFrame(TObject obj)
        {
            yield return null;

            obj.gameObject.SetActive(false);
            _pool.Push(obj);
        }

        protected abstract void Reinitialize(TObject obj);

        private TObject Create(Vector3 position, Quaternion rotation)
        {
            return _factory.Create(position, rotation, _container);
        }
    }
}
