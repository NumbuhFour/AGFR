using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Sign : Tile {
	
	public Sign(){
	}
	
	public Sign(JSONNode data, SpriteSheet sheet):base(data,sheet){
		
	}
	
	public override void OnUse(Entity e, TileData data){
		if(data["message"] != null)
			GameObject.FindGameObjectWithTag("Chat").GetComponent<ChatManager>().PushText((string)data["speaker"], (string)data["message"]);
	}
	
	public override void ReadData(JSONNode node, TileData data){
		if(node["message"] != null){
			data["speaker"] = node["speaker"].Value;
			data["message"] = node["message"].Value;
		}
	}
	
	public override TileData GetDefaultTileData(int x, int y){
		TileData data = new TileData(x,y);
		data["speaker"] = "[error] ";
		data["message"] = "error";
		return data;
	}
}
