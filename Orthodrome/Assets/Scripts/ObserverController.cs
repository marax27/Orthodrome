using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverController : MonoBehaviour {

	public float observerSpeed = 10f;
	public Transform objectOfInterestTransform;
	public float orbitRadius = 40f;

	public bool devMode = false;

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
			orbitPhase += observerSpeed * Time.deltaTime;
		}
	}

	void LateUpdate() {
		transform.LookAt(objectOfInterestTransform);
	}
}
