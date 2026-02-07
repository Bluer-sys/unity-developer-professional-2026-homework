using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "Game/ShipControllerViewConfig", fileName = "ShipControllerViewConfig")]
    public sealed class ShipViewConfig : ScriptableObject
    {
        [field: SerializeField] public Material MaterialPrefab { get; private set; }

        [field: Header("Damage")]
        [field: SerializeField] public AnimationCurve HitAnimationCurve { get; private set; }
        [field: SerializeField] public string HitPropertyName { get; private set; } = "_HitBlend";
        [field: SerializeField] public float HitDuration { get; private set; } = 0.2f;
        [field: SerializeField] public AudioClip DamageSfx { get; private set; }
        
        [field: Header("Move")]
        [field: SerializeField] public float MoveRotationAngle { get; private set; } = 30f;
        [field: SerializeField] public float MoveSpeed { get; private set; } = 5;

        [field: Header("Destroy")]
        [field: SerializeField] public ParticleSystem DestroyEffectPrefab { get; private set; }
        
    }
}
