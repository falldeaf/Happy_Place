using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableColliders : MonoBehaviour
{
	public Collider[] colliders;

	public void disable() {
		foreach(var col in colliders) {
			col.enabled = false;
		}
	}

	public void enable() {
		foreach(var col in colliders) {
			col.enabled = true;
		}
	}
}
