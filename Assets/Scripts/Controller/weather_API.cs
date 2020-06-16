using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Jint;

public class weather_API : MonoBehaviour {

	public static weather_API Instance{ get; private set; }

	public Weather_Scriptable[] weather_modes;

	public GameObject[] rain;
	public Material rain_glass;
	public Terrain terrain;
	public GameObject sheets;
	public WindZone wind;

	void Start() {
		Instance = this;
	}

	public void setup(Engine e) {
		e.SetValue("weather_preset", new Action<int>(setWeather));
	}

	public void setWeather(int index) {
		print(index.ToString());
		//print(weather_modes[index].name);
	}
	//RenderSettings.skybox =

	private void setRainGlass(int intensity) { 

	}

	private void setGrassWind(int intensity) {
		switch(intensity) {
			case 0:
				terrain.GetComponent<Terrain>().terrainData.wavingGrassSpeed    = 1f;
				terrain.GetComponent<Terrain>().terrainData.wavingGrassAmount   = 1f;
				terrain.GetComponent<Terrain>().terrainData.wavingGrassStrength = 1f;
				break;
			case 1:
				terrain.GetComponent<Terrain>().terrainData.wavingGrassSpeed    = 0.5f;
				terrain.GetComponent<Terrain>().terrainData.wavingGrassAmount   = 0.5f;
				terrain.GetComponent<Terrain>().terrainData.wavingGrassStrength = 0.5f;
				break;
			case 2:
				terrain.GetComponent<Terrain>().terrainData.wavingGrassSpeed    = 0.2f;
				terrain.GetComponent<Terrain>().terrainData.wavingGrassAmount   = 0.2f;
				terrain.GetComponent<Terrain>().terrainData.wavingGrassStrength = 0.2f;
				break;
		}
	}
}
