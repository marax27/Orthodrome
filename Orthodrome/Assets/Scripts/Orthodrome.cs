using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SphericalGeometry;

public class Orthodrome : GeoLine {

	public Orthodrome(SphericalGeometry outerInstance, GeoCoord startPoint, GeoCoord endPoint, int numberOfPoints)
		: base(outerInstance, startPoint, endPoint, numberOfPoints) { }

	protected override void ComputePoints() {
		// Precompute some values that will be used in later calculations.
		lat1 = Mathf.Deg2Rad * startPoint.latitude;
		lng1 = Mathf.Deg2Rad * startPoint.longitude;
		lat2 = Mathf.Deg2Rad * endPoint.latitude;
		lng2 = Mathf.Deg2Rad * endPoint.longitude;
		lng_diff = lng2 - lng1;

		coef1 = Mathf.Tan(lat2) * Mathf.Cos(lng1) - Mathf.Tan(lat1) * Mathf.Cos(lng2);
		coef2 = Mathf.Tan(lat2) * Mathf.Sin(lng1) - Mathf.Tan(lat1) * Mathf.Sin(lng2);

		// Fixed: wrong curve plotted when an orthodrome crossed the 180th meridian.
		if (Mathf.Abs(startPoint.longitude - endPoint.longitude) > 180f) {
			if (startPoint.longitude < 0f)
				lng1 += 360f * Mathf.Deg2Rad;
			else
				lng2 += 360f * Mathf.Deg2Rad;
		}

		// Compute all points except for the first and last one.
		for (int i = 1; i < Points.Length - 1; ++i) {
			var gc = ComputeSinglePoint(i / (float)Points.Length);
			Points[i] = outer.GeoCoord2LocalPoint(gc);
		}
	}

	private GeoCoord ComputeSinglePoint(float percent) {
		// If both points lie on the same meridian, the general equation doesn't apply.
		if (lng_diff == 0f)
			return new GeoCoord(Mathf.Lerp(lat1, lat2, percent), lng1);

		float lng = Mathf.Lerp(lng1, lng2, percent);

		// Compute tan(lat).
		float tan_lat = Mathf.Sin(lng) * coef1 - Mathf.Cos(lng) * coef2;
		tan_lat /= Mathf.Sin(lng_diff);

		return new GeoCoord(Mathf.Atan(tan_lat) * Mathf.Rad2Deg, lng * Mathf.Rad2Deg);
	}

	float lat1, lng1, lat2, lng2;
	float lng_diff;
	float coef1, coef2;
}
