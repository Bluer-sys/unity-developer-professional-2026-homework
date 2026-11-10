using Modules.FSM;
using UnityEngine;

namespace SampleGame
{
	public sealed class CharacterAi : MonoBehaviour
	{
		[SerializeField] private State _stateMachine;
		[SerializeField] private HealthComponent _healthComponent;

		private void OnEnable()
		{
			_healthComponent.OnDeath += OnDeath;
			_stateMachine.enabled = _healthComponent.IsAlive;
		}

		private void OnDisable()
		{
			_healthComponent.OnDeath -= OnDeath;
			_stateMachine.enabled = false;
		}

		private void OnDeath()
		{
			_stateMachine.enabled = false;
		}
	}
}
