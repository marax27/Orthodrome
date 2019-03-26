using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TimerNotification : BaseNotification {

	public Text titleText;
	public Text timerText;

	Color originalTitleTextColor;
	Color originalTimerTextColor;

	DateTime countdownTime;
	bool countdownTimeSet = false;

	/// <summary>
	/// Set countdown time. Return true on success, or false if parse failed.
	/// </summary>
	public bool SetCountdownTime(string _countdownTime) {
		bool result = DateTime.TryParse(_countdownTime, out countdownTime);
		countdownTimeSet = result;
		return result;
	}

	/// <summary>
	/// Set countdown time.
	/// </summary>
	public void SetCountdownTime(DateTime _countdownTime) {
		countdownTime = _countdownTime;
	}

	void Awake() {
		originalTitleTextColor = titleText.color;
		originalTimerTextColor = timerText.color;
	}

	void LateUpdate() {
		TimeSpan elapsed = GetElapsedTime();
		string txt = string.Format("{0:D1}:{1:D2}:{2:D2}:{3:D2}.{4}",
				elapsed.Days, elapsed.Hours, elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds / 100
			);
		timerText.text = txt;
	}

	TimeSpan GetElapsedTime() {
		if (!countdownTimeSet)
			return TimeSpan.Zero;

		DateTime now = DateTime.Now;
		if(now > countdownTime) {
			countdownTimeSet = false;
			return TimeSpan.Zero;
		}

		return countdownTime.Subtract(now);
	}

	protected override void SetContentFade(float percent) {
		titleText.color = Color.Lerp(Color.clear, originalTitleTextColor, percent);
		timerText.color = Color.Lerp(Color.clear, originalTimerTextColor, percent);
	}
}
