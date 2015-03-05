using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Button : Tile {
	
	public Button(JSONNode data, SpriteSheet sheet):base(data,sheet){
		
	}
	
	public override void OnEntityEnter(Entity e, TileData data){
		if(data["action"] != null) ((IButtonAction)data["action"]).OnEntityEnter(e);
	}
	public override void OnEntityExit(Entity e, TileData data){
		if(data["action"] != null) ((IButtonAction)data["action"]).OnEntityExit(e);
	}
	public override void Update(TileData data){
		if(data["action"] != null) ((IButtonAction)data["action"]).Update();
	}
	
	public override void ReadData(JSONNode node, TileData data){
		if(node["action"] != null){
			string actionName = node["action"].Value;
			switch(actionName){
			case "toggle lights":
				data["action"] = new ToggleLightsButton(data);
				break;
			}
		}
	}
}
