﻿using UnityEngine;
using System.Collections;
using SimpleJSON;

public class MapLoader : MonoBehaviour {
	public Map map;
	public EntityLayer entities;
	public EntityLayer sprites;
	
	public PremadeContainer entityList;
	
	public TextAsset blargh;
	private JSONNode data;
	private Vector2 dimensions;
	private Vector2 spawn;
	// Use this for initialization
	void Start () {
		data = JSON.Parse(blargh.text);
		ParseData();
		map.MarkDirty();
		entities.SpawnEntity(entityList["player"],spawn);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void ParseData(){
		dimensions = new Vector2(data["info"]["width"].AsInt, data["info"]["height"].AsInt);
		spawn = new Vector2(data["info"]["spawn"][0].AsInt, data["info"]["spawn"][1].AsInt);
		map.Init(dimensions, spawn);
		PopulateTiles(data["tiles"]);
		PopulateMap(data["map"]);
	}
	
	private void PopulateMap(JSONNode mapData){
		Vector2 offset = new Vector2((int)((Map.MAPDIM.x-this.dimensions.x)/2), //Offset to center
		                             (int)((Map.MAPDIM.y-this.dimensions.y)/2));
		for(int x = 0; x < dimensions.x; x++){
			for(int y = 0; y < dimensions.y; y++){
				map.SetTileAt(x + (int)offset.x,y + (int)offset.y,mapData[y][x]);
			}
		}
	}
	
	private void PopulateTiles(JSONNode tileJData){
		int count = tileJData.Count;
		for (int i = 0; i < count; i++){
			JSONNode t = tileJData[i];
			map.SetTile(t["name"], new Tile(t, map.sheet));
		}
	}
}