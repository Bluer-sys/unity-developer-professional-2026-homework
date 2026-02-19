using UnityEngine;

namespace Game.GameObjects
{
    public class BulletView : MonoBehaviour
    {
        [SerializeField] private GameObject _blueVFX;
        [SerializeField] private GameObject _redVFX;
        [SerializeField] private Bullet _bullet;

        private GameObject _explosionPrefab;

        private void OnEnable()
        {
            _bullet.OnDead += OnDead;
        }

        private void OnDisable()
        {
            _bullet.OnDead -= OnDead;
        }

        public void SetVfx(bool isRedVfx)
        {
            _blueVFX.SetActive(!isRedVfx);
            _redVFX.SetActive(isRedVfx);
        }

        public void SetExplosionPrefab(GameObject explosionPrefab)
        {
            _explosionPrefab = explosionPrefab;
        }
        
        private void OnDead(Bullet bullet)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        }
    }
}
