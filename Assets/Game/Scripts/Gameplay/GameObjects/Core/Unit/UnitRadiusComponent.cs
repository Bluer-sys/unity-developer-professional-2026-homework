using UnityEditor;
using UnityEngine;

namespace SampleGame
{
	public sealed class UnitRadiusComponent : MonoBehaviour
	{
		[field: SerializeField]
		public float Value { get; private set; }

		private void OnDrawGizmos()
		{
			Color prevColor = Gizmos.color;
			Gizmos.color = Color.white;
			Handles.DrawWireDisc(transform.position, Vector3.up, Value);
			Gizmos.color = prevColor;
		}
	}
}
