using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Weather/New Mode", order = 1)]
public class Weather_Scriptable : ScriptableObject
{
	public string quote;
	[Range(0.0f, 1.0f)] public float rain_intensity;
	[Range(0.0f, 1.0f)] public float wind_intensity;
	[Range(0.0f, 1.0f)] public int Dusk_Dawn;
	public Material skybox;
}