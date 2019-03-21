using System.Collections;
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

	//************************************************************

	void Awake() {
		canvas = FindObjectOfType<Canvas>();

		topArea = canvas.transform.Find("Top Area").gameObject;
		bottomArea = canvas.transform.Find("Bottom Area").gameObject;
		leftArea = canvas.transform.Find("Left Area").gameObject;
		rightArea = canvas.transform.Find("Right Area").gameObject;
		centerArea = canvas.transform.Find("Center Area").gameObject;
		frontArea = canvas.transform.Find("Front Area").gameObject;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			// Destroy random notification.
			System.Random rng = new System.Random();
			int ln = leftArea.transform.childCount;
			int n = ln + rightArea.transform.childCount;
			int i = rng.Next(n);

			GameObject sj;

			if (i < ln)
				sj = leftArea.transform.GetChild(i).gameObject;
			else
				sj = rightArea.transform.GetChild(i - ln).gameObject;

			print(string.Format($"~Destroy~ {ln}+rn={n}, {i} -- object: {sj}"));
			Destroy(sj);
		}
	}

	/// <summary>
	/// Attempt to display a notification. Return true or false, depending
	/// whether Scheduler succedeed or not.
	/// </summary>
	/// <param name="title">Notification title</param>
	/// <param name="description">Notification description text</param>
	/// <returns>Returns true if notification's been displayed succesfully.</returns>
	public bool Request(string title, string description) {

		// Create a Notification object.
		Notification obj = Instantiate(notificationPrefab) as Notification;
		obj.titleText.text = title;
		obj.descriptionText.text = description;

		// Update Canvas in order to update size of notification's RectTransform.
		Canvas.ForceUpdateCanvases();
		var height = obj.GetComponent<RectTransform>().rect.height * canvas.scaleFactor;

		// Assign a parent area.
		GameObject parent = null;
		//parent = (Random.Range(0f, 1f) > .5f) ? rightArea.transform : leftArea.transform;
		parent = leftArea;

		if(parent.GetComponent<RectTransform>().rect.height * canvas.scaleFactor - parent.GetComponent<VerticalLayoutGroup>().preferredHeight < height) {
			parent = rightArea;
		}

		obj.transform.SetParent(parent.transform);

		// Update parent parameters.
		// Canvas.ForceUpdateCanvases();
		parent.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();

		// On failure, destroy notification and return false.
		if (parent == null) {
			Destroy(obj);
			return false;
		}

		return true;
	}
}
