using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NotificationFactory is responsible for creation of objects of all classes
/// that derive from BaseNotification.
/// </summary>
public class NotificationFactory : MonoBehaviour {

	public TextNotification textNotification;

	/// <summary>
	/// Access an instance of NotificationFactory present at the scene.
	/// </summary>
	public static NotificationFactory Instance {
		// TODO: it almost certainly is a terrible solution.
		get {
			return GameObject.FindGameObjectWithTag("NotificationFactory").GetComponent<NotificationFactory>();
		}
	}

	public TextNotification GetText(string title, string description) {
		var result = Instantiate(textNotification) as TextNotification;
		result.titleText.text = title;
		result.descriptionText.text = description;
		return result;
	}
}
