using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverController : MonoBehaviour {

	[Tooltip("Linear (not angular) speed of the observer.")]
	public float observerSpeed = 10f;
	public float orbitRadius = 40f;

	public bool devMode = false;

	public Transform objectOfInterestTransform;
	public CenterArea centerArea;

	Vector3 velocity;
	float orbitPhase;  //angle that determines viewer's position around the planet

	void FixedUpdate() {
		if (devMode) {
			transform.position += observerSpeed * velocity.normalized * Time.fixedDeltaTime;
		} else {
			transform.position = objectOfInterestTransform.position + orbitRadius * new Vector3(Mathf.Cos(orbitPhase), 0f, Mathf.Sin(orbitPhase));
		}
	}

	void Update() {
		if (devMode) {
			// Position.
			int verticalAxisMove = (Input.GetKey(KeyCode.Space) ? 1 : 0) - (Input.GetKey(KeyCode.LeftShift) ? 1 : 0);

			velocity = transform.forward * Input.GetAxisRaw("Vertical")
					 + transform.right * Input.GetAxisRaw("Horizontal")
					 + transform.up * verticalAxisMove;
		} else {
			orbitPhase += observerSpeed / orbitRadius * Time.deltaTime;

			// Check if the planet fits within centerArea, and uses as much space as it can.
			if (!centerArea.IsEntireSphereInArea() || centerArea.GetSphereOccupancyPercentage() < .8f) {
				StopCoroutine("AdjustOrbitRadius");
				StartCoroutine("AdjustOrbitRadius");
			}
		}
	}

	void LateUpdate() {
		transform.LookAt(objectOfInterestTransform);
	}

	/// <summary>
	/// Increase or decrease radius of the observer's orbit so that the planet will occupy
	/// as much center area's space as possible, but it will be fully within this area.
	/// </summary>
	IEnumerator AdjustOrbitRadius() {
		bool initialState = centerArea.IsEntireSphereInArea();
		float radiusStep = .1f * (initialState ? -1 : +1);

		while (centerArea.IsEntireSphereInArea() == initialState) {
			orbitRadius += radiusStep;
			yield return new WaitForEndOfFrame();
		}

		if(!centerArea.IsEntireSphereInArea())
			orbitRadius += 2f * Mathf.Abs(radiusStep);
	}
}
