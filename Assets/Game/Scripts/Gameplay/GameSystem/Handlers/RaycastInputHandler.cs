using UnityEngine;

namespace SampleGame
{
	public sealed class RaycastInputHandler : InputHandler
	{
		[SerializeField]
		private Camera _camera;

		[SerializeField]
		private InputHandler _next;

		public override void Handle(ref InputContext context)
		{
			Ray ray = _camera.ScreenPointToRay(context.mousePosition);

			if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity,
					Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
			{
				Collider collider = raycastHit.collider;

				if (collider.CompareTag(GameObjectTags.Ground))
					context.point = raycastHit.point;
				else if (collider.CompareTag(GameObjectTags.Entity))
					context.target = collider.gameObject;
			}

			if (_next)
				_next.Handle(ref context);
		}
	}
}
