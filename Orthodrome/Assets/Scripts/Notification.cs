using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour {

	public Text titleText;
	public Text descriptionText;

	Color originalTitleTextColor;
	Color originalDescriptionTextColor;

	void Awake() {
		originalTitleTextColor = titleText.color;
		originalDescriptionTextColor = descriptionText.color;
	}

	/// <summary>
	/// Make a notification's contents (title and description) fade in.
	/// </summary>
	/// <param name="time"></param>
	public IEnumerator AnimateAppearance(float time) {
		float percent = 0f;
		float speed = 1f / time;

		while (percent < 1f) {
			percent += Time.deltaTime * speed;
			titleText.color = Color.Lerp(Color.clear, originalTitleTextColor, percent);
			descriptionText.color = Color.Lerp(Color.clear, originalDescriptionTextColor, percent);
			yield return null;
		}
	}
}
