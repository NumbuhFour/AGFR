using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Sign : Tile {
	
	public Sign(JSONNode data, SpriteSheet sheet):base(data,sheet){
		
	}
	
	public override void OnUse(Entity e, TileData data){
		GameObject.FindGameObjectWithTag("Chat").GetComponent<ChatManager>().PushText((string)data["speaker"], (string)data["message"]);
	}
	
	public override void ReadData(JSONNode node, TileData data){
		if(data["speaker"] != null){
			data["speaker"] = node["speaker"].Value;
			data["message"] = node["message"].Value;
		}
	}
}
