using UnityEngine;
using System.Collections;
using SimpleJSON;

public class LitTile : Tile {
	
	public LitTile(){
	}
	
	public LitTile(JSONNode data, SpriteSheet sheet):base(data,sheet){
		
	}
	
	public override void OnPlaced(TileData data){
		if(data["color"] != null){
			GameObject cont = GameObject.FindGameObjectWithTag("LightContainer");
			LightContainer container = cont.GetComponent<LightContainer>();
			TileLight light = container.AddLight(data.x, data.y); 
			data["light"] = light;
			light.Intensity = (float)data["intensity"];
			light.Range = (float)data["range"];
			light.Color = new Color((float)data["r"],(float)data["g"],(float)data["b"]);
		}
	}
	public override void OnRemoved(TileData data){
		if(data["light"] != null){
			GameObject.Destroy(((GameObject)data["light"]).gameObject);
			data["light"] = null;
		}
	}
	
	public override void ReadData(JSONNode node, TileData data){
		if(node["intensity"] != null){
			data["intensity"] = node["intensity"].AsFloat;
			data["range"] = node["range"].AsFloat;
			data["r"] = node["r"];
			data["g"] = node["g"];
			data["b"] = node["b"];
		}
		else{
			data["intensity"] = 0.05f;
			data["range"] = 0.05f;
			data["r"] = 1;
			data["g"] = 0.419f;
			data["b"] = 0;
		}
	}
	
	public override TileData GetDefaultTileData(int x, int y){
		TileData data = new TileData(x,y);
		data["intensity"] = 0.05f;
		data["range"] = 0.05f;
		data["r"] = 1;
		data["g"] = 0.419f;
		data["b"] = 0;
		return data;
	}
}
