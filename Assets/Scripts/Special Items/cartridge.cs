using UnityEngine;
using System;
using Jint;

public class cartridge : MonoBehaviour {

	private Engine engine;

	public Material led1; //Status LED
	public Material led2; //User configurable LED

	public GameObject port;
	public bool active = false;

	void Start() {
		ledSet(1, "off");
		ledSet(2, "off");
		weather_API.Instance.setWeather(0);

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
							 .SetValue("led1", new Action<string>(led1Set))
							 .SetValue("led2", new Action<string>(led2Set))
							 ;
							 
		//engine.GetValue("action");

		try {
			engine.Execute(@"
					led1('blue')
					log('Hello World')

					setInterval(function(){ log('OHHAI') }, 3000)

					function action(s) {
						log('js saw: ' + s)
					}
				");
		} catch (Jint.Parser.ParserException pEx) {
			ledSet(1, "red");
			print(pEx);
		} catch (Jint.Runtime.JavaScriptException rEx) {
			ledSet(1, "red");
			print(rEx);
		}
	}

	public void eject() {
		active = false;
	}

	public void led1Set(string color) {	ledSet(1, color); }
	public void led2Set(string color) {	ledSet(2, color); }
	
	private void ledSet(int index, string color) {
		print(index.ToString() + " : " + color);
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
		print(color.ToString());
		mat.SetColor("_EmissionColor", new Color(color.r * factor, color.g * factor, color.b * factor, 1));
	}
}
