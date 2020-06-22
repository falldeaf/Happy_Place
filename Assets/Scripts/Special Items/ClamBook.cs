using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamBook : MonoBehaviour {

	public GameObject port;
	public GameObject top;
	public AudioSource audio_source;

	public AudioClip open_sound;
	public AudioClip close_sound;
	public AudioClip click_sound;

	private bool is_open = false;
	private bool is_transforming = false;
	private bool is_booted = false;

	private Vector3 closed = new Vector3(180f, 180f, 0);
	private Vector3 opened = new Vector3(330f, 180f, 0);

	public void Start() {
		port.GetComponent<port>().registerDevice(gameObject);
	}

	public void action(string a) {
		switch(a) {
			case "swipe_up":
				if(!is_transforming && !is_open) {
					is_transforming = true;
					audio_source.PlayOneShot(open_sound, 1f);
					iTween.RotateAdd(top, iTween.Hash("x", 150f, "islocal", true, "easeType", 
							"easeInOutExpo", "loopType", "none", "time", 0.6f, 
							"oncomplete", "finishedOpening", "oncompletetarget", gameObject));
				}
				break;
			case "swipe_down":
				if(!is_transforming && is_open) {
					is_transforming = true;
					StartCoroutine(closedSoundDelay());
					iTween.RotateAdd(top, iTween.Hash("x", -150f, "islocal", true, "easeType", 
						"easeInOutExpo", "loopType", "none", "time", 0.6f, 
						"oncomplete", "finishedClosing", "oncompletetarget", gameObject));
				}
				break;
			case "trigger_click":
				print("Trigger!");
				if(port.GetComponent<port>().is_active) port.GetComponent<port>().eject();
				break;
			default: 
				audio_source.PlayOneShot(click_sound, 1f);
				break;
		}

		if(is_booted) {
			port.GetComponent<port>().deviceInput(a); 
		}
	}

	public void booting() { is_booted = true; }
	public void ejecting() { is_booted = false; }

	private void finishedOpening() {
		is_open = true;
		is_transforming = false;
		print("finished opening " + is_open.ToString());
	}

	IEnumerator closedSoundDelay() {
		yield return new WaitForSeconds(0.3f);
		audio_source.PlayOneShot(close_sound, 1f);
	}

	private void finishedClosing() {
		is_open = false;
		is_transforming = false;
		print("finished closing " + is_open.ToString());
	}
}
