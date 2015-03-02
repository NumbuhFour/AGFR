using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/Map/Map")]
public class Map : MonoBehaviour {

	public SpriteSheet sheet;

	public static Vector2 MAPDIM = new Vector2(25,25);
	
	public static Tile emptyTile;
	public static Tile errTile;
	
	private Dictionary<string, Tile> tiles;
	public string[,] map;
	private Vector2 dimensions;
	
	private TileData[,] tileData;
	private bool isDirty = false;
	
	public Tile GetTile(string name) { return tiles[name]; }
	public string[,] MapData { get { return map; } }
	public Color[] GetPixelsAt(int x, int y) { return GetVisibleTileAt(x,y).Pixels; }
	public Vector2 Dimensions { get { return dimensions; } }
	public bool IsDirty { get { return isDirty; } }
	
	private Vector2 offset;
	public Vector2 Offset { get { return offset; } }
	
	public void Init(Vector2 dimensions){
		sheet = Game.GameObject.GetComponent<SpriteSheet>();
		if(emptyTile == null){
			emptyTile = new Tile("empty",0, Color.clear, Color.white, 1, sheet);
			errTile = new Tile("error",0,Color.red, Color.white, 999, sheet);
		}
		
		this.dimensions = dimensions;
		offset = new Vector2((int)((MAPDIM.x-this.dimensions.x)/2), //Offset to center
		                     (int)((MAPDIM.y-this.dimensions.y)/2));
		tiles = new Dictionary<string,Tile>();
		map = new string[(int)dimensions.x,(int)dimensions.y];
		tileData = new TileData[(int)MAPDIM.x,(int)MAPDIM.y];
		foreach (EntityLayer elayer in this.GetComponentsInChildren<EntityLayer>()){
			elayer.Init(dimensions);
		}
	}
	
	public Tile GetTileAt(int x, int y) { 
		if(x < 0 || x >= Dimensions.x) return emptyTile;
		if(y < 0 || y >= Dimensions.y) return emptyTile;
		string t = map[x,y];
		if(t == null) return emptyTile;
		return tiles[t]; 
	}
	
	public Tile GetVisibleTileAt(int x, int y){
		if(x < 0 || x >= MAPDIM.x) return errTile;
		if(y < 0 || y >= MAPDIM.y) return errTile;
		
		Vector2 dim = ConvertSceneToWorld(new Vector2(x,y));
		return GetTileAt((int)dim.x, (int)dim.y);
	}
	
	/// Sets within the scale of the map's dimensions, not max dimensions
	public void SetTileAt(int x, int y, string tile){
		if(x < 0 || x >= this.dimensions.x || 
			y < 0 || y >= this.dimensions.y)
				return;
		map[x,y] = tile;
		this.MarkDirty();
	}
	
	public TileData GetTileDataAt(int x, int y){
		if(tileData[x,y] == null) tileData[x,y] = new TileData(x,y);
		return tileData[x,y];
	}
	public TileData GetTileDataVisibleAt(int x, int y){
		Vector2 dim = ConvertSceneToWorld(new Vector2(x,y));
		return GetTileDataAt((int)dim.x, (int)dim.y);
	}
	
	public void SetTile(string name, Tile t){
		if(t == null) tiles.Remove(name);
		else tiles[name] = t;
	}
	
	public void UseTile(Vector2 loc, Entity e){
		Tile t = GetTileAt((int)loc.x, (int)loc.y);
		TileData dat = GetTileDataAt((int)loc.x, (int)loc.y);
		t.OnUse(e, dat);
	}
	
	// Update is called once per frame
	void Update () {
		if(Game.Paused || Game.Mode != Game.GameMode.GAME) return;
		for(int x = 0; x<MAPDIM.x; x++){
			for(int y = 0; y<MAPDIM.y; y++){
				Tile t = GetTileAt (x,y);
				if(t == errTile || t == emptyTile) continue;
				
				TileData data = GetTileDataAt(x,y);
				t.Update(data);
			}
		}
	}
	
	public void MarkDirty(){ this.isDirty = true; }
	public void MarkClean(){ this.isDirty = false; }
	
	public bool IsScenePosWithinMap(Vector2 pos){
		Vector2 conv = ConvertSceneToWorld(pos);
		return conv.x >= 0 && conv.y >= 0 && 
			conv.x < dimensions.x && conv.y < dimensions.y;
	}
	
	public Vector2 ConvertSceneToWorld(Vector2 pos){
		return pos - offset;
	}
	
	public Vector2 ConvertWorldToScene(Vector2 pos){
		return pos + offset;
	}
}
