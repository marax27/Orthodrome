using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthController : MonoBehaviour
{
	public float rotationSpeed = 1.0f;

	void Update() {
		transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);
	}
}
