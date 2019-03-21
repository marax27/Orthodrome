using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NotifSender : MonoBehaviour
{
	public Scheduler scheduler;
	public Text outputText;
	public GameObject area;
	public Canvas canvas;

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			scheduler.Request(
				"Title",
				"If I manually deselect and reselect \"Preferred Height\" in the Layout Element component, the preferredHeight value changes."
			);
		}

		var r0 = area.GetComponent<RectTransform>().rect;
		string s0 = string.Format("rect: {{{0}, <b>{1}</b>}}", r0.width * canvas.scaleFactor, r0.height * canvas.scaleFactor);

		var vlg = area.GetComponent<VerticalLayoutGroup>();
		string s1 = string.Format("flex: {{{0}, <b>{1}</b>}}", vlg.flexibleWidth, vlg.flexibleHeight);
		string s2 = string.Format("pref: {{{0}, <b>{1}</b>}}", vlg.preferredWidth, vlg.preferredHeight);
		string s3 = string.Format("min: {{{0}, <b>{1}</b>}}", vlg.minWidth, vlg.minHeight);

		float freeHeight = r0.height * canvas.scaleFactor - vlg.preferredHeight;

		outputText.text = s0 + " --- " + s1 + " | " + s2 + " | " + s3 + " )( <b>" + freeHeight + "</b>";
    }
}
