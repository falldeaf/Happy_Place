using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Weather/New Mode", order = 1)]
public class Weather_Scriptable : ScriptableObject
{
	public string quote;
	[Range(0.0f, 1.0f)] public float rain_intensity;
	[Range(0.0f, 1.0f)] public float wind_intensity;
	[Range(0.0f, 1.0f)] public float Dusk_Dawn;
	
	[Header("0:nR nW 2:nR lW 3:lR lW 4:lR hW 5:hR lW 6:hR hW ")]
	[Range(0,6)] public int Wind_Rain_Audio_Snapshot;
	public Material skybox;
}