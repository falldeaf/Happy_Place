using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using Jint;

public class weather_API : MonoBehaviour {

	private bool currently_changing = false;
	private string json;

	public static weather_API Instance{ get; private set; }

	public Weather_Scriptable[] weather_modes;

	public GameObject sun;
	public Material skybox;
	public Material default_skybox;
	public ParticleSystem rain;
	public Material rain_glass;
	public Terrain terrain;
	public GameObject sheets;
	public WindZone wind;
	public AudioMixer mixer;
	public GameObject[] weather_audio_sources;
	public AnimationCurve rain_change;

	public float transition_time;

	void Start() {
		Instance = this;

		//Defaults (For items that save even during play)
		setDefaultSky(default_skybox);
		grassTweenCallback(0.2f);
		glassTweenCallback(0);
		
		//setWeather(0);
		json = "[";
		foreach(var scriptable in weather_modes) {
			json += "\"" + scriptable.quote + "\",";
		}
		json = json.TrimEnd(',');
		json += "]";
	}

	public void setup(Engine e) {
		e.Execute("weather_list = " + json);
		e.SetValue("switchweather",  new Action<int>(setWeather));
	}

	public void setWeather(int index) {
		if(currently_changing) return;
		iTween.Stop(gameObject);
		setSky(index);
		setRain(index);
		setRainGlass(index);
		setGrassWind(index);
		setWind(index);
		setSun(index);
		setWeatherAudio(index);
		StartCoroutine(changingWait());
	}
	//RenderSettings.skybox =

	IEnumerator changingWait() {
		currently_changing = true;
		yield return new WaitForSeconds(transition_time+2f);
	}

	private void setSun(int index) {
		iTween.RotateTo( sun, iTween.Hash(
			"x", Utility.convertRangeOne(weather_modes[index].Dusk_Dawn, 0, 360f),
			"time", transition_time,
			"onupdatetarget", gameObject,
			"easetype", iTween.EaseType.easeOutQuad
			)
		);
	}

	private void setWeatherAudio(int index) {
		float[] volumes =  new float[] {	weather_modes[index].light_wind,
											weather_modes[index].heavy_wind,
											weather_modes[index].light_rain,
											weather_modes[index].heavy_rain,
											weather_modes[index].daytime,
											weather_modes[index].nighttime };

		for(int i = 0; i<weather_audio_sources.Length; i++) {
			audioTween(weather_audio_sources[i], volumes[i]);
			//weather_audio_sources[i] = volumes[i];
		}
	}

	private void audioTween(GameObject go, float to) {
		iTween.AudioTo( go, iTween.Hash(
			"volume", to,
			"time", transition_time,
			"onupdatetarget", gameObject,
			"easetype", iTween.EaseType.easeOutQuad
			)
		);
	}

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
		weatherTween(Utility.convertRange(rain.emission.rate.constant, 10f, 5000f, 0, 1f), weather_modes[index].rain_intensity, "rainTweenCallback");
	}

	private void rainTweenCallback(float val) {
		if(rain.emission.rate.constant > 2000 && !sheets.activeSelf) {
			sheets.SetActive(true);
		} else if(rain.emission.rate.constant < 2000 && sheets.activeSelf) {
			sheets.SetActive(false);
		}
		var em = rain.emission;
		em.rateOverTime = Utility.convertRangeOne(rain_change.Evaluate(val), 10f, 5000f);
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
