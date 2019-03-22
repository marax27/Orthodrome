using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NotifSender : MonoBehaviour {
	public Scheduler scheduler;
	public GameObject sphere;

	public float sphereSpeed = 1f;

	System.Random rnd;
	string[] descriptions;

	void Awake() {
		rnd = new System.Random();

		descriptions = new string[] {
			"War is peace.\nFreedom is slavery.\nIgnorance is strength.",
			"For, after all, how do we know that two and two make four? Or that the force of gravity works? Or that the past is unchangeable? If both the past and the external world exist only in the mind, and if the mind itself is controllable – what then?",
			"Thoughtcrime does not entail death: thoughtcrime IS death",
			"Big Brother is Watching You.",
			"The Ministry of Peace concerns itself with war, the Ministry of Truth with lies, the Ministry of Love with torture and the Ministry of Plenty with starvation.",
			"Doublethink means the power of holding two contradictory beliefs in one’s mind simultaneously, and accepting both of them.",
			"Power is not a means; it is an end. One does not establish a dictatorship in order to safeguard a revolution; one makes the revolution in order to establish the dictatorship.",
			"Freedom is the freedom to say that two plus two make four. If that is granted, all else follows.",
			"War is a way of shattering to pieces, or pouring into the stratosphere, or sinking into the depths of the sea, materials which might otherwise be used to make the masses too comfortable, and hence, in the long run, too intelligent.",
			"Fancy thinking the Beast was something you could hunt and kill! You knew, didn’t you? I’m part of you? Close, close, close! I’m the reason why it’s no go? Why things are what they are?",
			"Maybe,\" he said hesitantly, \"maybe there is a beast.\" [...] \"What I mean is, maybe it's only us.",
			"Which is better--to have laws and agree, or to hunt and kill?",
			"I believe man suffers from an appalling ignorance of his own nature. I produce my own view in the belief that it may be something like the truth."
		};
	}

	void Start() {
		InvokeRepeating("SendRequest", 3f, 2f);
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			scheduler.PriorityMessage("2 MINUTES TO MIDNIGHT");
		}

		var sphereMovement = Input.GetAxisRaw("Vertical");
		sphere.transform.position += sphere.transform.forward * sphereMovement * sphereSpeed * Time.deltaTime;
	}

	void SendRequest() {
		scheduler.Request("Title", descriptions[rnd.Next(descriptions.Length)]);
	}
}