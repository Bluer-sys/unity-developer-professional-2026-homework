using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class HealthViewComponent : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer[] _renderers;
        [SerializeField] private Color _damagedColor = Color.red;
        [SerializeField] private float _frequency = 0.2f;
        [SerializeField] private int _loops = 3;

        private HealthComponent _healthComponent;

        private float _blend;
        private Tween _tween;

        [Inject]
        private void Construct(HealthComponent healthComponent)
        {
            _healthComponent = healthComponent;
        }

        private void OnValidate()
        {
            _renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _healthComponent.OnHealthDecreased += PlayBlink;
        }

        private void OnDisable()
        {
            _healthComponent.OnHealthDecreased -= PlayBlink;
            _tween?.Kill();
        }

        private void PlayBlink()
        {
            _tween?.Kill();
            _tween = DOTween.Sequence()
                            .Append(DOTween.To(() => _blend, x => _blend = x, 1f, _frequency))
                            .Append(DOTween.To(() => _blend, x => _blend = x, 0f, _frequency))
                            .SetLoops(_loops, LoopType.Restart)
                            .OnComplete(() => _tween = null);
        }

        private void LateUpdate()
        {
            Color color = Color.Lerp(Color.white, _damagedColor, _blend);
            
            foreach (SpriteRenderer r in _renderers)
                r.color = new Color(color.r, color.g, color.b, r.color.a);
        }
    }
}
