using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Door : Tile {
	
	public Door(JSONNode data, SpriteSheet sheet):base(data,sheet){
		
	}
	
	public override void OnEntityEnter(Entity e, TileData data){
		if(e.gameObject.tag == "Player"){
			Game.LevelSpawn = (Vector2)data["spawn"];
			Game.LoadLevel((string)data["level"]);
		}
	}
	
	public override void ReadData(JSONNode node, TileData data){
		data["level"] = node["level"].Value;
		JSONNode spawn = node["spawn"];
		
		data["spawn"] = new Vector2(spawn["x"].AsInt, spawn["y"].AsInt);
	}
}
