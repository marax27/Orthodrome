using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Provides functionality exclusive to the center area.
/// </summary>
public class CenterArea : Area {

	public GameObject sphere;

	public float diameterMultiplier = 1.2f;

	/// <summary>
	/// Get length of a side that surrounds the sphere (in screen units).
	/// Note: result isn't very accurate, but when it comes to
	/// realistic possible window sizes, it should work fine.
	/// </summary>
	float GetSphereSpan() {
		Vector3 rightDir = Camera.main.transform.right;
		Vector3 rightPoint = sphere.transform.position + diameterMultiplier * rightDir * sphere.transform.localScale.x;

		Vector2 screenSphereCenter = Camera.main.WorldToScreenPoint(sphere.transform.position);
		float result = 2 * ((Vector2)Camera.main.WorldToScreenPoint(rightPoint) - screenSphereCenter).magnitude;
		return result;
	}

	/// <summary>
	/// Return ratio between sphere span and it's maximum possible size.
	/// Ideally, it should return something close, but less than 1.
	/// </summary>
	public float GetSphereOccupancyPercentage() {
		return GetSphereSpan() / Mathf.Min(GetWidth(), GetHeight());
	}

	/// <summary>
	/// Check whether sphere is completely contained by the area.
	/// Returns false if the sphere intersects any of the area's boundaries.
	/// </summary>
	public bool IsEntireSphereInArea() {
		float span = GetSphereSpan();
		return span < GetWidth() && span < GetHeight();
	}
}
