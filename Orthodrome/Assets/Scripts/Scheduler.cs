using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Scheduler : MonoBehaviour {

	[Header("Screen Areas")]
	public GameObject topArea;
	public GameObject bottomArea;
	public GameObject leftArea;
	public GameObject rightArea;
	public GameObject centerArea;
	public GameObject frontArea;

	[Header("Prefabs")]
	public Notification notificationPrefab;

	//************************************************************

	/// <summary>
	/// Attempt to display a notification. Return true or false, depending
	/// whether Scheduler succedeed or not.
	/// </summary>
	/// <param name="title">Notification title</param>
	/// <param name="description">Notification description text</param>
	/// <returns>Returns true if notification's been displayed succesfully.</returns>
	public bool Request(string title, string description) {
		Notification obj = Instantiate(notificationPrefab, leftArea.transform) as Notification;
		obj.titleText.text = title;
		obj.descriptionText.text = description;

		return true;
	}
}
