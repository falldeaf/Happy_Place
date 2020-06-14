using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {
	public Light light;

	public void action(string a) {
		switch(a) {
			case "center_click":
				light.enabled = !light.enabled;
			break;
		}
	}
}
