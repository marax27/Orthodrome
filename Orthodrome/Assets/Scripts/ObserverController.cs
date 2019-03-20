using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverController : MonoBehaviour {

	public float observerSpeed = 10.0f;

	Vector3 velocity;

	void FixedUpdate() {
		transform.position += observerSpeed * velocity.normalized * Time.fixedDeltaTime;
	}

	void Update() {
		// Position.
		int verticalAxisMove = (Input.GetKey(KeyCode.Space) ? 1 : 0) - (Input.GetKey(KeyCode.LeftShift) ? 1 : 0);

		velocity = transform.forward * Input.GetAxisRaw("Vertical")
		         + transform.right * Input.GetAxisRaw("Horizontal")
		         + transform.up * verticalAxisMove;

		// Orientation.
		var pitch = Input.GetAxis("Mouse Y") * -150.0f * Time.deltaTime;
		var yaw = Input.GetAxis("Mouse X") * 150.0f * Time.deltaTime;
		var roll = (Input.GetMouseButton(1) ? yaw : 0.0f);
		if (roll != 0.0f)
			yaw = 0.0f;
		transform.Rotate(pitch, yaw, roll);
	}
}
