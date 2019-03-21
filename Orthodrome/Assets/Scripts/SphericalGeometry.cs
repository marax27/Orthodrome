using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides basic functionality involving geographic coordinates.
/// Meant for spherical objects, such as celestial bodies.
/// </summary>
public class SphericalGeometry : MonoBehaviour {

	float radius;

	void Awake() {
		radius = GetComponent<SphereCollider>().radius;
	}

	/// <summary>
	/// Get geographic coordinates of the worldPoint
	/// (or the point where the line worldPoint-sphere center
	/// intersects surface of the sphere).
	/// </summary>
	public GeoCoord WorldPoint2GeoCoord(Vector3 worldPoint) {
		// lp - Local Point.
		Vector3 lp = transform.InverseTransformPoint(worldPoint);
		GeoCoord result = new GeoCoord(
			Mathf.Atan2(lp.y, Mathf.Sqrt(lp.x*lp.x + lp.z*lp.z)),
			Mathf.Atan2(lp.z, lp.x)
		);

		// Radians to degrees.
		result.latitude *= Mathf.Rad2Deg;
		result.longitude *= Mathf.Rad2Deg;

		return result;
	}

	/// <summary>
	/// Get coordinates (in world space) of the point described
	/// by given geographic coordinates.
	/// </summary>
	public Vector3 GeoCoord2WorldPoint(GeoCoord coords) {
		float lat = coords.latitude * Mathf.Deg2Rad;
		float lng = coords.longitude * Mathf.Deg2Rad;

		Vector3 localPoint = new Vector3(
			Mathf.Cos(lng) * Mathf.Cos(lat),
			Mathf.Sin(lat),
			Mathf.Sin(lng) * Mathf.Cos(lat)
		);

		return transform.TransformPoint(radius * localPoint);
	}


	//------------------------------------------------------------

	/// <summary>
	/// Container for (latitude, longitude) coordinates (in degrees).
	/// </summary>
	public struct GeoCoord {
		public float latitude;   //phi
		public float longitude;  //lambda

		public GeoCoord(float lat, float lng) {
			latitude = lat;
			longitude = lng;
		}

		public override string ToString() {
			return string.Format("{{{0:0.0}, {1:0.0}}}", latitude, longitude);
		}
	}

	//------------------------------------------------------------
}
