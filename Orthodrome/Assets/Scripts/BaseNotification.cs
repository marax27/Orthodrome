using System.Collections;
using UnityEngine;

public abstract class BaseNotification : MonoBehaviour {

	/// <summary>
	/// Make a notification's contents (title and description) fade in.
	/// </summary>
	/// <param name="time"></param>
	public IEnumerator AnimateAppearance(float time) {
		float percent = 0f;
		float speed = 1f / time;

		while (percent < 1f) {
			percent += Time.deltaTime * speed;
			SetContentFade(percent);
			yield return null;
		}
	}

	/// <summary>
	/// Get width of a notification. Keep in mind the result should probably be multiplied 
	/// by canvas.scaleFactor. The notification doesn't have knowledge about canvas, though.
	/// </summary>
	public float GetUnscaledWidth() => GetComponent<RectTransform>().rect.width;

	/// <summary>
	/// Get height of a notification. Keep in mind the result should probably be multiplied 
	/// by canvas.scaleFactor. The notification doesn't have knowledge about canvas, though.
	/// </summary>
	public float GetUnscaledHeight() => GetComponent<RectTransform>().rect.height;

	/// <summary>
	/// Adjust children's opacity according to percent.
	/// percent == 0: transparent; percent == 1: opaque.
	/// </summary>
	/// <param name="percent"></param>
	protected abstract void SetContentFade(float percent);

}
