using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextNotification : BaseNotification {

	public Text titleText;
	public Text descriptionText;

	Color originalTitleTextColor;
	Color originalDescriptionTextColor;

	void Awake() {
		originalTitleTextColor = titleText.color;
		originalDescriptionTextColor = descriptionText.color;
	}

	protected override void SetContentFade(float percent) {
		titleText.color = Color.Lerp(Color.clear, originalTitleTextColor, percent);
		descriptionText.color = Color.Lerp(Color.clear, originalDescriptionTextColor, percent);
	}
}
