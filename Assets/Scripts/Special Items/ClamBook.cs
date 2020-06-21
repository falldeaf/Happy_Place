using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamBook : MonoBehaviour {

	public GameObject port;
	public GameObject top;

	private bool is_open = false;
	private bool is_booted = false;

	private Vector3 closed = new Vector3(180f, 180f, 0);
	private Vector3 opened = new Vector3(330f, 180f, 0);

	public void Start() {
		port.GetComponent<port>().registerDevice(gameObject);
	}

	public void action(string a) {
		switch(a) {
			case "center_click":
				print("saw center click in clam!");
				if(is_open) {
					//top.transform.localEulerAngles = closed;
					//top.transform.DOLocalRotate(closed, 1, RotateMode.Fast);
					iTween.RotateAdd(top, iTween.Hash("x", -150f, "islocal", true, "easeType", 
						"easeInOutExpo", "loopType", "none", "time", 0.6f, 
						"onStart", "playSound"));
				} else {
					//top.transform.localEulerAngles = opened;
					//top.transform.DOLocalRotate(opened, 1, RotateMode.Fast);
					iTween.RotateAdd(top, iTween.Hash("x", 150f, "islocal", true, "easeType", 
						"easeInOutExpo", "loopType", "none", "time", 0.6f, 
						"onStart", "playSound"));
				}
				is_open = !is_open;
				break;
			case "trigger_click":
				print("Trigger!");
				if(port.GetComponent<port>().is_active) port.GetComponent<port>().eject();
				break;
		}

		if(is_booted) { port.GetComponent<port>().deviceInput(a); }
	}

	public void booting() { is_booted = true; }
	public void ejecting() { is_booted = false; }

	private void playSound() {

	}
}
