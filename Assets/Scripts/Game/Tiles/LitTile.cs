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
			light.Color = (Color)data["color"];
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
			data["color"] = new Color(node["r"].AsFloat,node["g"].AsFloat,node["b"].AsFloat);
		}
		else{
			data["intensity"] = 0.05f;
			data["range"] = 0.05f;
			data["color"] = new Color(1,0.419f,0);
		}
	}
	
	public override TileData GetDefaultTileData(int x, int y){
		TileData data = new TileData(x,y);
		data["intensity"] = 0.05f;
		data["range"] = 0.05f;
		data["color"] = new Color(1,0.419f,0);
		return data;
	}
}
