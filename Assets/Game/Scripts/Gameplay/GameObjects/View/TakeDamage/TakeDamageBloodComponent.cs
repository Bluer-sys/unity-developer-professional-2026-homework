using UnityEngine;

namespace SampleGame
{
	public sealed class TakeDamageBloodComponent : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem _bloodVfx;

		[SerializeField]
		private ParticleSystem[] _bloodParticles;

		[SerializeField]
		private TeamConfig _teamConfig;

		[SerializeField]
		private TakeDamageComponent _takeDamageComponent;

		[SerializeField]
		private TeamComponent _teamComponent;

		private void OnEnable()
		{
			_takeDamageComponent.OnDamageTaken += OnDamageTaken;
		}

		private void OnDisable()
		{
			_takeDamageComponent.OnDamageTaken += OnDamageTaken;
		}

		private void OnDamageTaken(TakeDamageArgs obj)
		{
			Color color = _teamConfig.GetTeam(_teamComponent.Team).Material.color;

			foreach (ParticleSystem particle in _bloodParticles)
			{
				ParticleSystem.MainModule particleMain = particle.main;
				particleMain.startColor = color;
			}

			_bloodVfx.Play(withChildren: true);
		}
	}
}
