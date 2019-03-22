﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Provides functionality exclusive to the center area.
/// </summary>
public class CenterArea : Area {

	public GameObject sphere;

	public float diameterMultiplier = 1.2f;

	void Update() {
		// Signal if the sphere is out of bounds.
		GetComponent<Image>().color = IsEntireSphereInArea() ? Color.clear : Color.red;
	}

	/// <summary>
	/// Get length of a side that surrounds the sphere.
	/// </summary>
	float GetSphereSpan() {
		Vector3 rightDir = Camera.main.transform.right;
		Vector3 rightPoint = sphere.transform.position + diameterMultiplier * rightDir * sphere.transform.localScale.x;

		Vector2 screenSphereCenter = Camera.main.WorldToScreenPoint(sphere.transform.position);
		float result = 2 * ((Vector2)Camera.main.WorldToScreenPoint(rightPoint) - screenSphereCenter).magnitude;
		return result;
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
