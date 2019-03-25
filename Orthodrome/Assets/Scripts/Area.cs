using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a single area of the application's HUD. Provides basic functionality.
/// </summary>
public class Area : MonoBehaviour {

	private CanvasScaler parentCanvasScaler;

	private void Awake() {
		parentCanvasScaler = GetComponentInParent<CanvasScaler>();
	}

	public float GetWidth() {
		return GetComponent<RectTransform>().rect.width * parentCanvasScaler.scaleFactor;
	}

	public float GetHeight() {
		return GetComponent<RectTransform>().rect.height * parentCanvasScaler.scaleFactor;
	}
}
