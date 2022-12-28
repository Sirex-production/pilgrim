using UnityEngine;

public sealed class BoxDrawer : MonoBehaviour
{
	[SerializeField] private Vector3 offset;
	[SerializeField] private Vector3 size;
	[SerializeField] private Color color;

	private void OnDrawGizmos()
	{
		var localScale = transform.localScale;
		var targetSize = size;
		targetSize.x *= localScale.x;
		targetSize.y *= localScale.y;
		targetSize.z *= localScale.z;

		Gizmos.color = color;
		Gizmos.DrawWireCube(transform.position + offset, targetSize);
	}
}
