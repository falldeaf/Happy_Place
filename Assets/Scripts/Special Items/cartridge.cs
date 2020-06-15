using UnityEngine;
using System;
using Jint;

public class cartridge : MonoBehaviour {

	private Engine engine;

	private string program;

	public Material led1; //Status LED
	public Material led2; //User configurable LED
	public Renderer rend;

	public GameObject port;
	public bool active = false;

	public void initialize(string content, Texture label_image) {
		print("init!: " + content);
		if(label_image != null) { 
			print("Found label!");
			
			//Material temp = Instantiate<Material>(rend.materials[3]);
			Material[] temp_materials = rend.materials;
			temp_materials[3].SetTexture("_MainTex", label_image); 
			rend.materials = temp_materials;
		}
		program = content;
	}

	void Start() {

		ledSet(1, "off");
		ledSet(2, "off");

		//Temp program
		program = @"
					led('blue')
					log('Hello World')

					setInterval(function(){ log('OHHAI') }, 3000)

					function action(s) {
						log('js saw: ' + s)
					}
				";
	}

	public void action(string a){
		if(active) { print("made it to cart: " + a); engine.Invoke("action", a); }

		switch(a) {
			//Port is requesting insertion, respond!
			case "center_click":
				if(!active) {
					transform.parent.gameObject.GetComponent<hands>().releaseObjectSelf();
					port.GetComponent<port>().insert();
				}
				break;
		}
	}

	public void boot() {
		active = true;
		engine = new Engine().SetValue("log", new Action<object>(print)) 
							.SetValue("led", new Action<string>(led2Set))
							;
							 
		//engine.GetValue("action");

		try {
			engine.Execute(program);
		} catch (Jint.Parser.ParserException pEx) {
			ledSet(1, "red");
			print(pEx);
			return;
		} catch (Jint.Runtime.JavaScriptException rEx) {
			ledSet(1, "red");
			print(rEx);
			return;
		}
		ledSet(1, "blue");
	}

	public void eject() {
		active = false;
	}

	public void setWeather(string w) {
		weather_API.Instance.setWeather(0);
	}

	public void setRadioStation(string url) {
		print(url);
	}

	public void stopRadioStation() {
		print("stop radio");
	}

	public void led1Set(string color) {	ledSet(1, color); }
	public void led2Set(string color) {	ledSet(2, color); }
	
	private void ledSet(int index, string color) {
		var led = (index == 1) ? led1 : led2;
		switch(color) {
			case "off":
				setLedMat(led, new Color32(109, 109, 109, 255), 0);
				break;
			case "blue":
				setLedMat(led, new Color32(0, 159, 236, 255), 2);
				break;
			case "red":
				setLedMat(led, new Color32(224, 18, 18, 255), 0.5f);
				break;
		}
	}

	private void setLedMat(Material mat, Color32 color, float intensity) {
		float factor = intensity; //(intensity>0) ? Mathf.Pow(2,intensity) : 0;
		mat.SetColor("_EmissionColor", new Color(color.r * factor, color.g * factor, color.b * factor, 1));
	}

	public void setLabelBase64(string image) {
		//string b64_string = imag;
		byte[] bytes = System.Convert.FromBase64String(image);
		var tex = new Texture2D(1,1);
		tex.LoadImage( bytes);
		//label.SetTexture("_MainTexture", tex);
	}
}
