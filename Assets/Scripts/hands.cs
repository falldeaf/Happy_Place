using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class hands : MonoBehaviour {

	public GameObject model;
	public SteamVR_Input_Sources handType;
	public SteamVR_Behaviour_Pose controllerPose;
	public SteamVR_Action_Boolean grabAction;
	public SteamVR_Action_Boolean centerClickAction;
	public SteamVR_Action_Boolean left_ClickAction;
	public SteamVR_Action_Boolean right_ClickAction;
	public SteamVR_Action_Boolean top_ClickAction;
	public SteamVR_Action_Boolean bottom_ClickAction;
	public SteamVR_Action_Boolean trigger_ClickAction;

	private GameObject collidingObject;
	private GameObject objectInHand;

	private void SetCollidingObject(Collider col) {
		if (collidingObject || !col.GetComponent<Rigidbody>()) {
			return;
		}
		collidingObject = col.gameObject;
	}

	public void OnTriggerEnter(Collider other) {
		SetCollidingObject(other);
	}

	public void OnTriggerStay(Collider other) {
		SetCollidingObject(other);
	}

	public void OnTriggerExit(Collider other) {
		if (!collidingObject) {
			return;
		}

		collidingObject = null;
	}

	private void GrabObject() {
		objectInHand = collidingObject;
		collidingObject = null;

		objectInHand.GetComponent<Rigidbody>().isKinematic = true;
		if(objectInHand.GetComponent<DisableColliders>() != null) objectInHand.GetComponent<DisableColliders>().enable();
		//objectInHand.GetComponent<Rigidbody>().detectCollisions = false;
		objectInHand.transform.parent = transform;

		if(objectInHand.GetComponent<Reorient>() != null) {
			objectInHand.GetComponent<Reorient>().pickedUp();
			objectInHand.GetComponent<Reorient>().is_held = true;
			//model.GetComponent<Renderer>().enabled  = objectInHand.GetComponent<Reorient>().visible_controller;
			if(!objectInHand.GetComponent<Reorient>().visible_controller) hideController(true);
		}
	}

	private FixedJoint AddFixedJoint() {
		FixedJoint fx = gameObject.AddComponent<FixedJoint>();
		fx.breakForce = 20000;
		fx.breakTorque = 20000;
		return fx;
	}

	//The object released itself and will handle it's own state, just set this hand to empty
	public void releaseObjectSelf() {
		hideController(false);
		objectInHand = null;
	}

	private void ReleaseObject() {
		/*
		if (GetComponent<FixedJoint>()) {
			//GetComponent<FixedJoint>().connectedBody = null;
			//Destroy(GetComponent<FixedJoint>());
		}
		*/
		objectInHand.transform.parent = null;
		hideController(false);
		if(objectInHand.GetComponent<Reorient>() != null) objectInHand.GetComponent<Reorient>().is_held = false;
		if(objectInHand.GetComponent<DisableColliders>() != null) objectInHand.GetComponent<DisableColliders>().enable();
		objectInHand.GetComponent<Rigidbody>().isKinematic = false;
		//objectInHand.GetComponent<Rigidbody>().detectCollisions = true;
		objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
		objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
		objectInHand = null;
	}

	private void hideController(bool h) {
		var rends = model.GetComponentsInChildren<Renderer>();
		foreach(Renderer rend in rends) {
			rend.enabled = !h;
		}
	}


	void Update() {
		/*
		if (grabAction.GetLastStateDown(handType)) {
			if (collidingObject) {
				print("Grab!");
				GrabObject();
			}
		}
		*/
		if (centerClickAction.GetLastStateDown(handType) && objectInHand != null) {
			objectInHand.SendMessage("action", "center_click");
		}
		
		if (left_ClickAction.GetLastStateDown(handType) && objectInHand != null) {
			objectInHand.SendMessage("action", "left_click");
		}

		if (right_ClickAction.GetLastStateDown(handType) && objectInHand != null) {
			objectInHand.SendMessage("action", "right_click");
		}

		if (top_ClickAction.GetLastStateDown(handType) && objectInHand != null) {
			objectInHand.SendMessage("action", "top_click");
		}

		if (bottom_ClickAction.GetLastStateDown(handType) && objectInHand != null) {
			print("bottom!");
			objectInHand.SendMessage("action", "bottom_click");
		}

		if (trigger_ClickAction.GetLastStateDown(handType) && objectInHand != null) {
			objectInHand.SendMessage("action", "trigger_click");
		}

		if (grabAction.GetLastStateUp(handType)) {
			if (objectInHand) {
				ReleaseObject();
			} else {
				if (collidingObject && collidingObject.transform.parent == null) {
					GrabObject();
				}
			}
		}		
	}
}
