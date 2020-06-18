using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Jint;

public class weather_API : MonoBehaviour {

	public static weather_API Instance{ get; private set; }

	public Weather_Scriptable[] weather_modes;

	public Material skybox;
	public Material default_skybox;
	public ParticleSystem rain;
	public Material rain_glass;
	public Terrain terrain;
	public GameObject sheets;
	public WindZone wind;

	public float transition_time;

	void Start() {
		Instance = this;

		//Defaults (For items that save even during play)
		setDefaultSky(default_skybox);
		grassTweenCallback(0.2f);
		glassTweenCallback(0);
		
		setWeather(0);
	}

	public void setup(Engine e) {
		e.SetValue("weather_preset", new Action<int>(setWeather));
	}

	public void setWeather(int index) {
		iTween.Stop(gameObject);
		setSky(index);
		setRain(index);
		setRainGlass(index);
		setGrassWind(index);
		setWind(index);
	}
	//RenderSettings.skybox =

	private void weatherTween(float from, float to, string callback) {
		iTween.ValueTo( gameObject, iTween.Hash(
			"from", from,
			"to", to,
			"time", transition_time,
			"onupdatetarget", gameObject,
			"onupdate", callback,
			"easetype", iTween.EaseType.easeOutQuad
			)
		);
	}

	private void setWind(int index) {
		weatherTween(wind.windMain, Utility.convertRangeOne(weather_modes[index].wind_intensity, 0, 4f), "windMainTweenCallback");
		weatherTween(wind.windTurbulence, Utility.convertRangeOne(weather_modes[index].wind_intensity, 0, 0.8f), "windTurbulenceTweenCallback");
	}

	private void windMainTweenCallback(float val) {
		wind.windMain = val;
	}

	private void windTurbulenceTweenCallback(float val) {
		wind.windTurbulence = val;
	}

	private void setRain(int index) {
		weatherTween(rain.emission.rate.constant, Utility.convertRangeOne(weather_modes[index].rain_intensity, 10f, 5000f), "rainTweenCallback");
	}

	private void rainTweenCallback(float val) {
		var em = rain.emission;
		em.rateOverTime = val;
	}

	private void setRainGlass(int index) {
		weatherTween(rain_glass.GetFloat("_Size"), weather_modes[index].rain_intensity, "glassTweenCallback");
	}

	private void glassTweenCallback(float val) {
		rain_glass.SetFloat("_Size", val);
		rain_glass.SetFloat("_Blur", val);
	}

	//SKY MATERIAL///////////////////
	private void setDefaultSky(Material mat) {
		skybox.SetTexture("_FrontTex",  mat.GetTexture("_FrontTex"));
		skybox.SetTexture("_BackTex",   mat.GetTexture("_BackTex"));
		skybox.SetTexture("_LeftTex",   mat.GetTexture("_LeftTex"));
		skybox.SetTexture("_RightTex",  mat.GetTexture("_RightTex"));
		skybox.SetTexture("_UpTex",     mat.GetTexture("_UpTex"));
		skybox.SetTexture("_DownTex",   mat.GetTexture("_DownTex"));
		skybox.SetTexture("_FrontTex2", mat.GetTexture("_FrontTex"));
		skybox.SetTexture("_BackTex2",  mat.GetTexture("_BackTex"));
		skybox.SetTexture("_LeftTex2",  mat.GetTexture("_LeftTex"));
		skybox.SetTexture("_RightTex2", mat.GetTexture("_RightTex"));
		skybox.SetTexture("_UpTex2",    mat.GetTexture("_UpTex"));
		skybox.SetTexture("_DownTex2",  mat.GetTexture("_DownTex"));
	}

	private void setSky(int index) {
		skybox.SetTexture("_FrontTex", skybox.GetTexture("_FrontTex2"));
		skybox.SetTexture("_BackTex",  skybox.GetTexture("_BackTex2"));
		skybox.SetTexture("_LeftTex",  skybox.GetTexture("_LeftTex2"));
		skybox.SetTexture("_RightTex", skybox.GetTexture("_RightTex2"));
		skybox.SetTexture("_UpTex",    skybox.GetTexture("_UpTex2"));
		skybox.SetTexture("_DownTex",  skybox.GetTexture("_DownTex2"));

		skybox.SetTexture("_FrontTex2", weather_modes[index].skybox.GetTexture("_FrontTex"));
		skybox.SetTexture("_BackTex2",  weather_modes[index].skybox.GetTexture("_BackTex"));
		skybox.SetTexture("_LeftTex2",  weather_modes[index].skybox.GetTexture("_LeftTex"));
		skybox.SetTexture("_RightTex2", weather_modes[index].skybox.GetTexture("_RightTex"));
		skybox.SetTexture("_UpTex2",    weather_modes[index].skybox.GetTexture("_UpTex"));
		skybox.SetTexture("_DownTex2",  weather_modes[index].skybox.GetTexture("_DownTex"));

		weatherTween(0, 1f, "skyTweenCallBack");
	}

	void skyTweenCallBack( float val ) {
		skybox.SetFloat("_Blend", val);
	}


	private void setGrassWind(int index) {
		var intensity = weather_modes[index].wind_intensity;
		weatherTween(terrain.terrainData.wavingGrassSpeed, weather_modes[index].rain_intensity, "grassTweenCallback");
	}

	private void grassTweenCallback(float val) {
		terrain.terrainData.wavingGrassSpeed    = val;
		terrain.terrainData.wavingGrassAmount   = val;
		terrain.terrainData.wavingGrassStrength = val;
	}
}
