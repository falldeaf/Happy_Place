using UnityEngine;
using UnityEditor.Presets;

[CreateAssetMenu(fileName = "Data", menuName = "Weather/New Mode", order = 1)]
public class Weather_Scriptable : ScriptableObject
{
	public Material skybox;
	public Preset rain;
	public bool rain_sheets;
	public Preset wind;
	public Preset grass_wind;
}