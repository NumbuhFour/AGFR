using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Door : Tile {
	
	public Door(JSONNode data, SpriteSheet sheet):base(data,sheet){
		
	}
	
	public override void OnEntityEnter(Entity e, TileData data){
		if(e.gameObject.tag == "Player"){
			Game.LevelSpawn = new Vector2((int)data["spawnx"],(int)data["spawny"]);
			Game.LoadLevel((string)data["level"]);
		}
	}
	
	public override void ReadData(JSONNode node, TileData data){
		data["level"] = node["level"].Value;
		
		data["spawnx"] = node["spawnx"].AsInt;
		data["spawny"] = node["spawny"].AsInt;
	}
}
