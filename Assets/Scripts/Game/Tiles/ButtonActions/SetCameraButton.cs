using UnityEngine;
using System.Collections;
using SimpleJSON;

public class SetCameraButton : IButtonAction
{
	private TileData data;
	private Vector2 pos;
	private int y;
	public SetCameraButton(JSONNode node, TileData data){
		this.data = data;
		pos = new Vector2(node["camx"].AsInt,node["camy"].AsInt);
	}
	
	public void OnEntityEnter(Entity e){
		e.map.CamLoc = pos;
	}
	public void OnEntityExit(Entity e){
	}
	public void Update(){
		
	}
}

