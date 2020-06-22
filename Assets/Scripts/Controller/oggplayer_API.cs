using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using Jint;
using ATL;

public class oggplayer_API : MonoBehaviour
{
	public static oggplayer_API Instance{ get; private set; }

	private List<OGG> playlist;
	private string json;
	
	public AudioSource speaker1;
	public AudioSource speaker2;

	private AudioClip myClip;

	void Start() {
		Instance = this;

 		playlist = new List<OGG>();
		json = "[";

		foreach (string file in System.IO.Directory.GetFiles(Application.dataPath + "/StreamingAssets/ogg/")) {
			if(Path.GetExtension(file).Equals(".ogg")) {

				print(file);
				var ogg = new OGG();
				ogg.path = file;

				Track resp = new Track(file);
				if (resp != null) {
					ogg.songname = resp.Title;
					ogg.artist = resp.Artist;
					ogg.album = resp.Album;
					print(resp.Title + " : " + resp.Artist + " : " + resp.Album);
				}
				json += JsonUtility.ToJson(ogg) + ",";
				playlist.Add(ogg);
			}
		}
		json = json.TrimEnd(',');
		json += "]";
	}

	public void setup(Engine e) {
		print(json);
		e.Execute("playlist = " + json);
		e.SetValue("play",  new Action<int>(playFromIndex));
		e.SetValue("stop",  new Action<int>(stopMusic));
	}

	public void playFromIndex(int index) {
		StartCoroutine(playAudio(playlist[index].path));
	}

	public void stopMusic(int i) {
		speaker1.Stop();
		speaker2.Stop();
	}

	IEnumerator playAudio(string path) {
		yield return new WaitForEndOfFrame();
		UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.OGGVORBIS);
		
		//uwr.uri = new Uri(uwr.uri.AbsoluteUri.Replace("http://localhost", "file://"));
		//uwr.url = uwr.url.Replace("http://localhost", "file://");
		//if (Debug.isDebugBuild) Debug.LogFormat("Loading audio file for {0} from {1} ", App.State.TextPieceToRead.Title, uwr.url);
		
		DownloadHandlerAudioClip dhac = uwr.downloadHandler as DownloadHandlerAudioClip;
		dhac.streamAudio = true;
		yield return uwr.SendWebRequest();

		if (uwr.isHttpError || uwr.isNetworkError)
		{
			Debug.LogErrorFormat("Could not load audio file for {0}. Error: {1}", path,  uwr.error);
			yield break;
		}

		var _currentClip = dhac.audioClip;
		
		if (_currentClip != null)
		{
			if (Debug.isDebugBuild) Debug.Log ("Clip load type: " + _currentClip.loadType);
			if (Debug.isDebugBuild) Debug.Log ("Clip load state: " + _currentClip.loadState);
			//_audioSource.clip = _currentClip;
			//_audioSource.time = 0f;
			//_audioSource.Play();

			//print(path);
			//AudioClip clip = Resources.Load<AudioClip>(path);
			speaker1.PlayOneShot(_currentClip);
			speaker2.PlayOneShot(_currentClip);
		}
	}

	void Update() {
		speaker2.timeSamples = speaker1.timeSamples;
	}
}

[Serializable]
public class OGG {
	public string path;
	public string songname;
	public string artist;
	public string album;
}