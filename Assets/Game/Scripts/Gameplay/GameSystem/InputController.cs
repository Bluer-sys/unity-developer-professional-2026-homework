using UnityEngine;

namespace SampleGame
{
	public sealed class InputController : MonoBehaviour
	{
		[SerializeField]
		private InputHandler _handler;

		[SerializeField]
		private HealthComponent _healthComponent;

		private void Update()
		{
			if (!_healthComponent || _healthComponent.IsDead)
				return;

			InputContext context = new InputContext();
			_handler.Handle(ref context);
		}
	}
}
