using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reorient : MonoBehaviour {

	public bool visible_controller = false;
	public Vector3 local_position;
	public Vector3 local_rotation;
	public bool is_held = false;

	public void pickedUp() {
		is_held = true;
		transform.localPosition = local_position;
		//transform.localEulerAngles = local_rotation;
		//transform.DOLocalRotate(local_rotation, 1, RotateMode.Fast);
		iTween.RotateTo(gameObject, iTween.Hash("rotation", local_rotation, "islocal", true, "easeType", 
						"easeInOutExpo", "loopType", "none", "time", 0.6f, 
						"onStart", "playSound"));
		//transform.DOLocalMove(local_position, 1, true);
	}
}
