using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Weather/New Mode", order = 1)]
public class Weather_Scriptable : ScriptableObject
{
	public Material skybox;
	public GameObject rain;
	public bool rain_sheets;
	public bool rain_drops;
	public GameObject wind;
	public int grass_wind;
}