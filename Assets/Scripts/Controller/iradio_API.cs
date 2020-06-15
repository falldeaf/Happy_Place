using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ID3Tag;

public class iradio_API : MonoBehaviour
{
	public List<MP3> playlist = new List<MP3>();
	public AudioSource speaker;
	public string url;

	private AudioClip myClip;


	void Start() {
		ID3Tag.Core.TagParser.ID3v1Parser reader = new ID3Tag.Core.TagParser.ID3v1Parser();
		foreach (string file in System.IO.Directory.GetFiles(Application.dataPath + "/StreamingAssets/mp3/")) {
			if(Path.GetExtension(file).Equals(".mp3")) {

				var mp3 = new MP3();
				mp3.path = file;

				ID3Tag.Core.ID3TagObject resp = reader.Read(file);
				if (resp != null) {
					mp3.songname = resp.Title;
					mp3.artist = resp.Artist;
					mp3.album = resp.Album;
				}
				print(file);
				playlist.Add(mp3);
			}
		}

		print(playlist);
	}

}

public class MP3 {
	public string path { get; set; }
	public string songname { get; set; }
	public string artist { get; set; }
	public string album { get; set; }
}