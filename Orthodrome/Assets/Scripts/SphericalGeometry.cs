using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides basic functionality involving geographic coordinates.
/// Meant for spherical objects, such as celestial bodies.
/// </summary>
public class SphericalGeometry : MonoBehaviour {

	//------------------------------------------------------------

	/// <summary>
	/// Get geographic coordinates of the worldPoint, or the point where
	/// the line worldPoint-sphere center intersects surface of the sphere).
	/// </summary>
	public GeoCoord WorldPoint2GeoCoord(Vector3 worldPoint) {
		Vector3 localPoint = transform.InverseTransformPoint(worldPoint);
		return LocalPoint2GeoCoord(localPoint);
	}

	/// <summary>
	/// Get coordinates (in world space) of the point described
	/// by given geographic coordinates.
	/// </summary>
	public Vector3 GeoCoord2WorldPoint(GeoCoord coords) {
		Vector3 localPoint = GeoCoord2LocalPoint(coords);
		return transform.TransformPoint(localPoint);
	}


	//------------------------------------------------------------

	/// <summary>
	/// Get geographic coordinates of the localPoint (in a sphere's
	/// coordinate system), or the point where the line localPoint-sphere center 
	/// intersects surface of the sphere).
	/// </summary>
	public GeoCoord LocalPoint2GeoCoord(Vector3 localPoint) {
		GeoCoord result = new GeoCoord(
			Mathf.Atan2(localPoint.y, Mathf.Sqrt(localPoint.x * localPoint.x + localPoint.z * localPoint.z)),
			Mathf.Atan2(localPoint.z, localPoint.x)
		);

		// Radians to degrees.
		result.latitude *= Mathf.Rad2Deg;
		result.longitude *= Mathf.Rad2Deg;

		return result;
	}

	/// <summary>
	/// Get coordinates (in sphere's coordinate system)
	/// of the point described by given geographic coordinates.
	/// </summary>
	public Vector3 GeoCoord2LocalPoint(GeoCoord coords) {
		float lat = coords.latitude * Mathf.Deg2Rad;
		float lng = coords.longitude * Mathf.Deg2Rad;

		Vector3 localPoint = new Vector3(
			Mathf.Cos(lng) * Mathf.Cos(lat),
			Mathf.Sin(lat),
			Mathf.Sin(lng) * Mathf.Cos(lat)
		);

		return localPoint;
	}

	//------------------------------------------------------------

	public Vector3 GetWorldPointAboveGround(Vector3 worldPoint, float height) {
		Vector3 localPoint = transform.InverseTransformPoint(worldPoint);
		return transform.TransformPoint(GetLocalPointAboveGround(localPoint, height));
	}

	public Vector3 GetLocalPointAboveGround(Vector3 localPoint, float height) {
		Vector3 r = localPoint;
		return (1 + height) * r;
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

	/// <summary>
	/// A base class for any line of interest on the sphere's surface.
	/// </summary>
	public abstract class GeoLine {
		public GeoCoord[] Points { get; protected set; }

		public GeoLine(SphericalGeometry outerInstance, GeoCoord startPoint, GeoCoord endPoint, int numberOfPoints) {
			if (numberOfPoints < 2)
				throw new System.ArgumentException($"Invalid number of points. Expected: >= 2, provided: {numberOfPoints}");

			outer = outerInstance;

			Points = new GeoCoord[numberOfPoints];
			Points[0] = startPoint;
			Points[numberOfPoints - 1] = endPoint;

			ComputePoints();
		}

		/// <summary>
		/// Compute all points along the line of interest. Note that
		/// the start & end points are already known.
		/// </summary>
		protected abstract void ComputePoints();

		private SphericalGeometry outer;
	}

	//------------------------------------------------------------
}
