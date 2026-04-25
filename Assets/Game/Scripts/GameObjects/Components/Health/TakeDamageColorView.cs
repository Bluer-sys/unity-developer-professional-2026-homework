using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class TakeDamageColorView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer[] _renderers;

        [SerializeField]
        private Color _damagedColor = Color.red;

        [SerializeField]
        private float _frequency = 0.2f;

        [SerializeField]
        private int _loops = 3;

        private HealthComponent _healthComponent;

        private Color _baseColor;
        private float _blend;
        private Tween _tween;
        private float _previousHealth = float.MaxValue;

        [Inject]
        private void Construct(HealthComponent healthComponent)
        {
            _healthComponent = healthComponent;
        }

        private void Reset()
        {
            _renderers = this.GetComponentsInChildren<SpriteRenderer>();
        }

        private void Awake()
        {
            _baseColor = _renderers is { Length: > 0 } ? _renderers[0].color : Color.white;
        }

        private void OnEnable()
        {
            _previousHealth = _healthComponent.CurrentHealth;
            _healthComponent.OnHealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            _healthComponent.OnHealthChanged -= OnHealthChanged;
            _tween?.Kill();
        }

        private void OnHealthChanged(float current)
        {
            if (current < _previousHealth)
                this.PlayBlink();

            _previousHealth = current;
        }

        private void PlayBlink()
        {
            _tween?.Kill();
            _tween = DOTween.Sequence()
                .Append(DOTween.To(() => _blend, x => _blend = x, 1f, _frequency))
                .Append(DOTween.To(() => _blend, x => _blend = x, 0f, _frequency))
                .SetLoops(_loops, LoopType.Restart);
        }

        private void LateUpdate()
        {
            Color color = Color.Lerp(_baseColor, _damagedColor, _blend);
            foreach (SpriteRenderer r in _renderers)
                r.color = new Color(color.r, color.g, color.b, r.color.a);
        }
    }
}
