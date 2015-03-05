using UnityEngine;
using System.Collections;
using SimpleJSON;

[AddComponentMenu("Scripts/Map/Map Loader")]
public class MapLoader : MonoBehaviour {
	public Map map;
	public EntityLayer entities;
	public EntityLayer sprites;
	
	public PremadeContainer entityList;
	
	private JSONNode data;
	private Vector2 dimensions;
	private Vector2 spawn;
	
	public void Load(TextAsset mapFile){
		if(entities)entities.Clear();
		if(sprites)sprites.Clear();
		data = JSON.Parse(mapFile.text);
		ParseData ();
		map.MarkDirty();
		
		if(Game.Mode == Game.GameMode.GAME){
			if(Game.LevelSpawn.x != -1) spawn = Game.LevelSpawn;
			Game.Player = entities.SpawnEntity(entityList["player"],spawn);
			map.CenterCameraOn(spawn);
		}
	}
	
	private void ParseData(){
		dimensions = new Vector2(data["info"]["width"].AsInt, data["info"]["height"].AsInt);
		spawn = new Vector2(data["info"]["spawn"][0].AsInt, data["info"]["spawn"][1].AsInt);
		map.Init(dimensions);
		PopulateTiles(data["tiles"]);
		PopulateMap(data["map"]);
		if(data["entities"] != null 
			&& Game.Mode == Game.GameMode.GAME)  //Temporary until editor entites
			PopulateEntities(data["entities"]);
	}
	
	private void PopulateMap(JSONNode mapData){
		for(int x = 0; x < dimensions.x; x++){
			for(int y = 0; y < dimensions.y; y++){
				JSONNode tile = mapData[(int)dimensions.y-y-1][x];
				if(tile.GetType() == typeof(SimpleJSON.JSONClass)){
					map.SetTileAt(x,y, tile["style"].Value);
					Tile t = map.GetTile(tile["style"].Value);
					t.ReadData(tile, map.GetTileDataAt(x,y));
				}
				else {
					map.SetTileAt(x,y, tile);
				}
			}
		}
	}
	
	private void PopulateTiles(JSONNode tileJData){
		int count = tileJData.Count;
		for (int i = 0; i < count; i++){
			JSONNode t = tileJData[i];
			string name = t["name"].Value;
			string type = t["type"].Value;
			if(type == null) type = "tile";
			map.SetTile(name, MakeTileInstance(type, t, map.sheet));
		}
	}
	
	private void PopulateEntities(JSONNode tileJData){
		int count = tileJData.Count;
		for (int i = 0; i < count; i++){
			JSONNode t = tileJData[i];
			int x = t["x"].AsInt;
			int y = t["y"].AsInt;
			string name = t["name"].Value;
			entities.SpawnEntity(entityList[name],new Vector2(x,y));
		}
	}
	
	public static Color ReadColor(JSONNode colorNode){
		float r = colorNode["r"].AsFloat;
		float g = colorNode["g"].AsFloat;
		float b = colorNode["b"].AsFloat;
		float a = colorNode["a"].AsFloat;
		if(r > 1 || g > 1 || b > 1){
			r /= 255f;
			g /= 255f;
			b /= 255f;
		}
		if(a > 1){ //Alpha seperate because I forget to change that sometimes
			a /= 255f;
		}
		return new Color(r,g,b,a);
		
	}
	
	public static Tile MakeTileInstance(string type, JSONNode file, SpriteSheet sheet){
		switch(type){
		default: return new Tile(file,sheet);
		case "button": return new Button(file,sheet);
		case "sign": return new Sign(file,sheet);
		case "door": return new Door(file,sheet);
		}
	}
}
