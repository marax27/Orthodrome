using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SphericalGeometry;

public class Orthodrome : GeoLine {

	public Orthodrome(SphericalGeometry outerInstance, GeoCoord startPoint, GeoCoord endPoint, int numberOfPoints)
		: base(outerInstance, startPoint, endPoint, numberOfPoints) { }

	protected override void ComputePoints() {
		// Precompute some values that will be used in later calculations.
		lat1 = Mathf.Deg2Rad * Points[0].latitude;
		lng1 = Mathf.Deg2Rad * Points[0].longitude;
		lat2 = Mathf.Deg2Rad * Points[Points.Length - 1].latitude;
		lng2 = Mathf.Deg2Rad * Points[Points.Length - 1].longitude;
		lng_diff = Mathf.Abs(lng2 - lng1);

		// Compute all points except for the first and last one.
		for (int i = 1; i < Points.Length - 1; ++i)
			Points[i] = ComputeSinglePoint(i / (float)Points.Length);
	}

	private GeoCoord ComputeSinglePoint(float percent) {
		// If both points lie on the same meridian, the general equation doesn't apply.
		if (lng_diff == 0f)
			return new GeoCoord(Mathf.Lerp(lat1, lat2, percent), lng1);

		// TODO: vulnerable to the -180/+180 meridian.
		float lng = Mathf.Lerp(lng1, lng2, percent);

		// Compute tan(lat).
		float tan_lat = Mathf.Sin(lng) * (Mathf.Tan(lat2) * Mathf.Cos(lng1) - Mathf.Tan(lat1) * Mathf.Cos(lng2));
		tan_lat -= Mathf.Cos(lng) * (Mathf.Tan(lat2) * Mathf.Sin(lng1) - Mathf.Tan(lat1) * Mathf.Sin(lng2));
		tan_lat /= Mathf.Sin(lng2 - lng1);

		return new GeoCoord(Mathf.Atan(tan_lat) * Mathf.Rad2Deg, lng * Mathf.Rad2Deg);
	}

	float lat1, lng1, lat2, lng2;
	float lng_diff;
}
