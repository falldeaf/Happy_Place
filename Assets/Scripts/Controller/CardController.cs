using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CardController : MonoBehaviour
{
	public Transform[] slots;
	public GameObject cart_prefab;
	private List<GameObject> carts = new List<GameObject>();

	void Start() {
		int index = 0;

		foreach (string file in System.IO.Directory.GetFiles(Application.dataPath + "/StreamingAssets/scripts/")) {
			if(Path.GetExtension(file).Equals(".js")) {
				var cart = Instantiate(cart_prefab, slots[index].position, slots[index].rotation);
				
				string program_string = File.ReadAllText(file);
				string image_path = Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + ".png";
				print(image_path);

				if(File.Exists( image_path ) ) {
					cart.GetComponent<cartridge>().initialize(program_string, loadTextureFromFile(image_path));
				} else {
					cart.GetComponent<cartridge>().initialize(program_string, null);
				}
				
				index++;
			}
		}
	}

	public static Texture2D loadTextureFromFile(string filename) {
		// "Empty" texture. Will be replaced by LoadImage
		Texture2D texture = new Texture2D(4, 4);

		FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
		byte[] imageData = new byte[fs.Length];
		fs.Read(imageData, 0, (int)fs.Length);
		texture.LoadImage(imageData);

		return texture;
	}

}
