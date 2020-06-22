using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyPhone : MonoBehaviour {
	public AudioSource audio_source;
	public Reorient orienter;

	private bool changed = false;

	void Update() {
		if(changed != orienter.is_held) {
			changed = orienter.is_held;
			if(orienter.is_held) {
				audio_source.Play();
			} else {
				audio_source.Stop();
			}
		}
	}
}
