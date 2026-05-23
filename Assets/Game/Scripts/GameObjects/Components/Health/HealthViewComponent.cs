using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class HealthViewComponent : IInitializable, IDisposable, ILateTickable
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField] public Color DamagedColor { get; private set; } = Color.red;
            [field: SerializeField] public float Frequency { get; private set; } = 0.2f;
            [field: SerializeField] public int Loops { get; private set; } = 3;
        }

        private readonly Settings _settings;
        private readonly HealthComponent _healthComponent;
        private readonly SpriteRenderer[] _renderers;

        private float _blend;
        private Tween _tween;

        public HealthViewComponent(
            Settings settings, 
            HealthComponent healthComponent, 
            TransformComponent transformComponent)
        {
            _settings = settings;
            _healthComponent = healthComponent;

            _renderers = transformComponent.Transform.GetComponentsInChildren<SpriteRenderer>();
        }

        public void Initialize()
        {
            _healthComponent.OnHealthDecreased += PlayBlink;
        }

        public void Dispose()
        {
            _healthComponent.OnHealthDecreased -= PlayBlink;
            _tween?.Kill();
        }

        public void LateTick()
        {
            Color color = Color.Lerp(Color.white, _settings.DamagedColor, _blend);

            foreach (SpriteRenderer r in _renderers)
                r.color = new Color(color.r, color.g, color.b, r.color.a);
        }

        private void PlayBlink()
        {
            _tween?.Kill();
            _tween = DOTween.Sequence()
                            .Append(DOTween.To(() => _blend, x => _blend = x, 1f, _settings.Frequency))
                            .Append(DOTween.To(() => _blend, x => _blend = x, 0f, _settings.Frequency))
                            .SetLoops(_settings.Loops, LoopType.Restart)
                            .OnComplete(() => _tween = null);
        }
    }
}
