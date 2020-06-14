using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class port : MonoBehaviour {

	public GameObject insertion_position;

	private GameObject device;
	private GameObject current_card;
	private bool is_active = false;

	public void registerDevice(GameObject go) { device = go; }

	public void OnTriggerEnter(Collider other) {
		//print("Saw a " + other.gameObject.name + " : " + other.gameObject.tag);
		if(other.gameObject.tag == "tinyc" && other.GetComponent<Reorient>().is_held == true && is_active == false) {
			current_card = other.gameObject;
			current_card.GetComponent<cartridge>().port = gameObject;
		}
	}

	public void OnTriggerStay(Collider other) {
		if(other.gameObject.tag == "tinyc" && other.GetComponent<Reorient>().is_held == true && is_active == false) {
			//orient card correctly in space when near port
			var card = other.gameObject.transform.GetChild(0);
			//card.transform.position = insertion_position.transform.position;
			card.transform.position = Vector3.Lerp(card.transform.position, insertion_position.transform.position, 0.5f);
			//card.transform.rotation = insertion_position.transform.rotation;
			card.transform.rotation = Quaternion.Lerp(card.transform.rotation, insertion_position.transform.rotation, 0.5f);
		}
	}

	public void OnTriggerExit(Collider other) {
		if(other.gameObject == current_card && is_active == false) {
			var card = other.gameObject.transform.GetChild(0);
			card.transform.localRotation = Quaternion.identity;
			card.transform.localPosition = Vector3.zero;
			current_card = null;
		}
	}

	public void insert() {
		is_active = true;
		current_card.GetComponent<BoxCollider>().enabled = false;
		current_card.transform.parent = transform;
		current_card.transform.GetChild(0).transform.localPosition = Vector3.zero;
		current_card.transform.GetChild(0).transform.localRotation = Quaternion.identity;
		current_card.transform.localPosition = insertion_position.transform.localPosition;
		current_card.transform.localRotation = insertion_position.transform.localRotation;
		iTween.MoveTo(current_card, iTween.Hash("position", Vector3.zero, "islocal", true, "easeType", 
												"easeInOutExpo", "loopType", "none", "time", 1.4f, 
												"oncomplete", "initiateBoot", "oncompletetarget" , this.gameObject));
	}

	public void deviceInput(string a) {
		if(is_active) { current_card.GetComponent<cartridge>().action(a); }
	}

	public void initiateBoot() {
		print("boot!");
		device.SendMessage("booting");
		current_card.GetComponent<cartridge>().boot();
	}
}
