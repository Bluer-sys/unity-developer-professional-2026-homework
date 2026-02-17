using UnityEngine;

namespace Game.GameObjects.Bullet
{
    public class BulletView : MonoBehaviour
    {
        [SerializeField] private GameObject _blueVFX;
        [SerializeField] private GameObject _redVFX;

        private Bullet _bullet;
        private BulletConfig _config;

        private GameObject _explosionPrefab;

        public void Construct(Bullet bullet)
        {
            _bullet = bullet;
            
            _bullet.OnDead += OnDead;
        }
        
        private void OnDestroy()
        {
            _bullet.OnDead += OnDead;
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
        
        private void OnDead()
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        }
    }
}
