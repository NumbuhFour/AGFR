using UnityEngine;
using System.Collections;
using SimpleJSON;

[AddComponentMenu("Scripts/LevelEditor/Preset Loader")]
public class PresetLoader : MonoBehaviour {

	public TextAsset presetFile;
	public EditorUI invMan;

	public EditorItem[] presets;

	// Use this for initialization
	void Start () {
		LoadPresets();
	}
	
	public void LoadPresets(){
		JSONNode data = JSON.Parse(presetFile.text);
		
		JSONNode tiles = data["tiles"];
		int count = tiles.Count;
		presets = new EditorItem[count];
		for (int i = 0; i < count; i++){
			JSONNode t = tiles[i];
			string name = t["name"].Value;
			string type = t["type"].Value;
			if(type == null) type = "tile";
			int spriteIndex = t["sprite"].AsInt;
			int solidity = t["solidity"].AsInt;
			Color mainColor = MapLoader.ReadColor(t["main_color"]);
			Color swapColor = MapLoader.ReadColor(t["swap_color"]);
			
			presets[i] = new EditorItem(name, type, spriteIndex, mainColor, swapColor, solidity);
			invMan.AddTilePreset(presets[i]);
		}
	}
}
