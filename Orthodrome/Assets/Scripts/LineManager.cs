using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineManager : MonoBehaviour {
	public SphericalGeometry earth;
	public Text coordText;

	public LineRenderer linePrefab;

	bool earthHasMouseFocus = false;

	SphericalGeometry.GeoCoord focusEarthPoint;
	bool focusEarthPointSet = false;

	LineRenderer lr;

	private void Start() {
		lr = Instantiate(linePrefab, earth.transform) as LineRenderer;
	}

	private void Update() {
		// Geographic coordinates.
		RaycastHit hitInfo;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hitInfo)) {
			// Display updated geographic coordinates of a hit point.
			SphericalGeometry.GeoCoord gc = earth.WorldPoint2GeoCoord(hitInfo.point);
			coordText.text = "Coordinates: " + gc.ToString();
			coordText.text += string.Format("\n{0} -- {1} -- {2} -- {3}",
				hitInfo.point,
				earth.GeoCoord2WorldPoint(earth.WorldPoint2GeoCoord(hitInfo.point)),
				earth.GeoCoord2WorldPoint(earth.LocalPoint2GeoCoord(earth.transform.InverseTransformPoint(hitInfo.point))),
				earth.transform.TransformPoint(earth.GeoCoord2LocalPoint(earth.LocalPoint2GeoCoord(earth.transform.InverseTransformPoint(hitInfo.point))))
			);

			earthHasMouseFocus = true;

			// Handle mouseLine.
			if (Input.GetMouseButtonDown(0)) {
				// GeoCoord should do a better job than Vector3,
				// since it should not care about Earth's rotation.
				focusEarthPoint = gc;
				focusEarthPointSet = true;
			}

		} else if (earthHasMouseFocus) {
			earthHasMouseFocus = false;
			coordText.text = "Coordinates: N/A";
		}
	}

	private void LateUpdate() {
		if (!focusEarthPointSet)
			return;

		RaycastHit hitInfo;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hitInfo)) {
			SphericalGeometry.GeoCoord gc = earth.WorldPoint2GeoCoord(hitInfo.point);

			const int N = 100;
			System.Func<float, float> evaluator = (x) => (.95f * x * (1 - x));
			Orthodrome ort = new Orthodrome(earth, focusEarthPoint, gc, N);
			ort.AdjustHeight(evaluator);
			lr.positionCount = N;
			lr.SetPositions(ort.Points);
		}
	}
}
