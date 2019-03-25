using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scheduler : MonoBehaviour {

	[Header("Prefabs")]
	public TextNotification notificationPrefab;

	[Header("Others")]
	public float frontAreaTransitionTime = 1f;
	public float notificationTransitionTime = .3f;

	private Canvas canvas;

	private Area topArea;
	private Area bottomArea;
	private Area leftArea;
	private Area rightArea;
	private Area centerArea;
	private Area frontArea;

	private Text frontAreaText;
	private Color frontAreaTextFinalColor;
	private Color frontAreaFinalBackgroundColor;

	private Queue<BaseNotification> notificationQueue = new Queue<BaseNotification>();

	//************************************************************

	void Awake() {
		canvas = FindObjectOfType<Canvas>();

		// NOTE: Is this a recommended way of obtaining gameObjects in Unity?
		topArea = canvas.transform.Find("Top Area").GetComponent<Area>();
		bottomArea = canvas.transform.Find("Bottom Area").GetComponent<Area>();
		leftArea = canvas.transform.Find("Left Area").GetComponent<Area>();
		rightArea = canvas.transform.Find("Right Area").GetComponent<Area>();
		centerArea = canvas.transform.Find("Center Area").GetComponent<Area>();
		frontArea = canvas.transform.Find("Front Area").GetComponent<Area>();

		//frontAreaTextObject = frontArea.transform.Find("Front Area Text").gameObject;
		frontAreaText = frontArea.transform.GetComponentInChildren<Text>(true);
		frontAreaTextFinalColor = frontAreaText.color;

		frontAreaFinalBackgroundColor = frontArea.GetComponent<Image>().color;
	}

	/// <summary>
	/// Attempt to display a notification. Return true or false, depending
	/// whether Scheduler succedeed or not.
	/// </summary>
	/// <param name="title">Notification title</param>
	/// <param name="description">Notification description text</param>
	/// <returns>Returns true if notification has been displayed succesfully.</returns>
	public bool Request(BaseNotification notification) {

		// Update Canvas in order to update size of notification's RectTransform.
		Canvas.ForceUpdateCanvases();
		float notificationHeight = NotificationHeight(notification);

		// Choose area in which the notification will be displayed.
		Area notificationParent = null;
		Area[] possibleParentChoices = new Area[] { leftArea, rightArea };
		List<Area> appropriateParentChoices = new List<Area>();

		while(appropriateParentChoices.Count == 0) {
			// Pick only those potential parents that can fit a child.
			foreach (var i in possibleParentChoices) {
				if (notificationHeight <= FreeSpaceInArea(i))
					appropriateParentChoices.Add(i);
			}

			if (appropriateParentChoices.Count == 0) {
				// There are no areas that can fit a notification.
				// Remove first notification in the queue and try again.

				if(notificationQueue.Count == 0) {
					// Beyond reason! No notifications left, and still not enough free space.
					Destroy(notification.gameObject);
					return false;
				}

				DestroyImmediate(notificationQueue.Dequeue().gameObject);
				Canvas.ForceUpdateCanvases();
			}
		}

		// Set notification's parent - randomly chosen from the list of possible choices.
		var idx = (new System.Random()).Next(appropriateParentChoices.Count);
		notificationParent = appropriateParentChoices[idx];
		notification.transform.SetParent(notificationParent.transform);

		// Update parent layout.
		notificationParent.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();

		notificationQueue.Enqueue(notification);
		StartCoroutine(notification.AnimateAppearance(notificationTransitionTime));

		return true;
	}

	/// <summary>
	/// Display a priority message in the front area.
	/// </summary>
	/// <param name="message"></param>
	public bool PriorityMessage(string message) {
		try {
			frontAreaText.text = message;

			frontArea.gameObject.SetActive(true);
			StartCoroutine(AnimateFrontAreaAppearance(frontAreaTransitionTime, true));
		}catch(UnityException) {
			return false;
		}

		return true;
	}

	/// <summary>
	/// Smoothly bring the front area to the screen. If 'appear' is true,
	/// the area will appear on the screen; otherwise, it will vanish.
	/// </summary>
	/// <param name="time"></param>
	/// <param name="appear"></param>
	IEnumerator AnimateFrontAreaAppearance(float time, bool appear) {
		if (!frontArea.gameObject.activeSelf)
			frontArea.gameObject.SetActive(true);

		Color startAreaColor = appear ? Color.clear : frontAreaFinalBackgroundColor;
		Color startTextColor = appear ? Color.clear : frontAreaTextFinalColor;
		Color finalAreaColor = appear ? frontAreaFinalBackgroundColor : Color.clear;
		Color finalTextColor = appear ? frontAreaTextFinalColor : Color.clear;

		float percent = 0f;
		float animationSpeed = 1f / time;

		while (percent < 1f) {
			percent += Time.deltaTime * animationSpeed;
			frontArea.GetComponent<Image>().color = Color.Lerp(startAreaColor, finalAreaColor, percent);
			frontAreaText.color = Color.Lerp(startTextColor, finalTextColor, percent);
			yield return null;
		}

		if (!appear)
			frontArea.gameObject.SetActive(false);
	}

	/// <summary>
	/// Get notification height, in pixels.
	/// </summary>
	/// <param name="notif"></param>
	float NotificationHeight(BaseNotification notif) {
		return canvas.scaleFactor * notif.GetUnscaledHeight();
	}

	/// <summary>
	/// Get height (in pixels) of the remaining free space in the area.
	/// </summary>
	/// <param name="area"></param>
	float FreeSpaceInArea(Area area) {
		return area.GetHeight() - area.GetComponent<VerticalLayoutGroup>().preferredHeight;
	}
}
