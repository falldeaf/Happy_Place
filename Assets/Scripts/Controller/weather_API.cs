using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weather_API : MonoBehaviour {

	public static weather_API Instance{ get; private set; }

	public Weather_Scriptable[] weather_modes;

	public Terrain terrain;
	public ParticleSystem rain;
	public GameObject sheets;
	public WindZone wind;

	void Start() {
		Instance = this;
	}

	public void setWeather(int index) {
		print(weather_modes[index].name);
	}
	//RenderSettings.skybox =
}
