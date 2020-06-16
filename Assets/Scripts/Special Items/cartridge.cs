using UnityEngine;
using System;
using Jint;

public class cartridge : MonoBehaviour {

	private Engine engine;

	private string program;

	public Renderer rend;
	public GameObject port;
	public bool active = false;

	public void initialize(string content, Texture label_image) {
		if(label_image != null) { 
			Material[] temp_materials = rend.materials;
			temp_materials[3].SetTexture("_MainTex", label_image); 
			rend.materials = temp_materials;
		}
		program = content;
	}

	void Start() {
		led1Set("off");
		led2Set("off");
	}

	public void action(string a){
		if(active) { 
			engine.Invoke("action", a); 
		}

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
							.SetValue("write1", new Action<string>(setText1))
							.SetValue("write2", new Action<string>(setText2))
							.SetValue("setup", new Action<string>(setup))
							;

		try {
			engine.Execute(program);
		} catch (Jint.Parser.ParserException pEx) {
			led1Set("red");
			print(pEx);
			return;
		} catch (Jint.Runtime.JavaScriptException rEx) {
			led1Set("red");
			print(rEx);
			return;
		}
		ledSet(1, "blue");
	}

	public void eject() {
		active = false;
	}

	public void setup(string api_name) {
		switch(api_name) {
			case "oggplayer":
				oggplayer_API.Instance.setup(engine);
				break;
			case "weather":
				weather_API.Instance.setup(engine);
				break;
		}
	}

	public void setText1(string t) { port.GetComponent<port>().setText1(t); }
	public void setText2(string t) { port.GetComponent<port>().setText2(t); }

	public void led1Set(string color) {	ledSet(1, color); }
	public void led2Set(string color) {	ledSet(2, color); }
	
	private void ledSet(int index, string color) {
		switch(color) {
			case "off":
				setLedMat(index, new Color(0.05f, 0.05f, 0.05f, 1), 0);
				break;
			case "blue":
				setLedMat(index, new Color(0.02f, 0.6f, 1f, 1), 0.5f);
				break;
			case "red":
				setLedMat(index, new Color(1f, 0, 0, 1), 0.5f);
				break;
			case "green":
				setLedMat(index, new Color(0.02f, 1f, 0.05f, 1), 0.5f);
				break;
			case "purple":
				setLedMat(index, new Color(0.8f, 0, 1f, 1), 0.5f);
				break;
		}
	}

	private void setLedMat(int mat_index, Color color, float intensity) {
		Material[] temp_materials = rend.materials;
		float factor = intensity;
		temp_materials[mat_index].SetColor("_EmissionColor", new Color(color.r, color.g, color.b, 1));
		
		rend.materials = temp_materials;
	}
}
