using UnityEngine;

namespace SampleGame
{
	public sealed class DeathSFXBehaviour : MonoBehaviour
	{
		[SerializeField]
		private AudioSource _audioSource;

		[SerializeField]
		private AudioClip _deathClip;

		[SerializeField]
		private HealthComponent _healthComponent;

		private void OnEnable()
		{
			_healthComponent.OnDeath += OnDeath;
		}

		private void OnDisable()
		{
			_healthComponent.OnDeath -= OnDeath;
		}

		private void OnDeath()
		{
			_audioSource.PlayOneShot(_deathClip);
		}
	}
}
