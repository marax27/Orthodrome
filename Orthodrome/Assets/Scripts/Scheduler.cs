using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scheduler : MonoBehaviour {

	[Header("Prefabs")]
	public Notification notificationPrefab;

	private Canvas canvas;

	private GameObject topArea;
	private GameObject bottomArea;
	private GameObject leftArea;
	private GameObject rightArea;
	private GameObject centerArea;
	private GameObject frontArea;

	private Text frontAreaText;

	//************************************************************

	void Awake() {
		canvas = FindObjectOfType<Canvas>();

		// NOTE: Is this a recommended way of obtaining gameObjects in Unity?
		topArea = canvas.transform.Find("Top Area").gameObject;
		bottomArea = canvas.transform.Find("Bottom Area").gameObject;
		leftArea = canvas.transform.Find("Left Area").gameObject;
		rightArea = canvas.transform.Find("Right Area").gameObject;
		centerArea = canvas.transform.Find("Center Area").gameObject;
		frontArea = canvas.transform.Find("Front Area").gameObject;

		//frontAreaTextObject = frontArea.transform.Find("Front Area Text").gameObject;
		frontAreaText = frontArea.transform.GetComponentInChildren<Text>(true);
	}

	/// <summary>
	/// Attempt to display a notification. Return true or false, depending
	/// whether Scheduler succedeed or not.
	/// </summary>
	/// <param name="title">Notification title</param>
	/// <param name="description">Notification description text</param>
	/// <returns>Returns true if notification has been displayed succesfully.</returns>
	public bool Request(string title, string description) {

		// Create Notification object.
		Notification notification = Instantiate(notificationPrefab) as Notification;
		notification.titleText.text = title;
		notification.descriptionText.text = description;

		// Update Canvas in order to update size of notification's RectTransform.
		Canvas.ForceUpdateCanvases();
		float notificationHeight = canvas.scaleFactor * notification.GetComponent<RectTransform>().rect.height;

		// Choose area in which the notification will be displayed.
		GameObject notificationParent = null;
		GameObject[] possibleParentChoices = new GameObject[2] { leftArea, rightArea };
		List<GameObject> appropriateParentChoices = new List<GameObject>();

		// Pick only those potential parents that can fit a child.
		foreach(var i in possibleParentChoices) {
			if (notificationHeight <= i.GetComponent<RectTransform>().rect.height * canvas.scaleFactor - i.GetComponent<VerticalLayoutGroup>().preferredHeight)
				appropriateParentChoices.Add(i);
		}

		if(appropriateParentChoices.Count == 0) {
			// There are no areas that can fit a notification.
			// TODO: handle this more elaborately.
			// For now, scheduler will simply return false.
			Destroy(notification.gameObject);
			return false;
		}

		// Set notification's parent - randomly chosen from the list of possible choices.
		var idx = (new System.Random()).Next(appropriateParentChoices.Count);
		notificationParent = appropriateParentChoices[idx];
		notification.transform.SetParent(notificationParent.transform);

		// Update parent layout.
		notificationParent.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();

		return true;
	}

	/// <summary>
	/// Display a priority message in the front area.
	/// </summary>
	/// <param name="message"></param>
	public bool PriorityMessage(string message) {
		try {
			frontAreaText.text = message;

			frontArea.SetActive(true);
		}catch(UnityException) {
			return false;
		}

		return true;
	}
}
