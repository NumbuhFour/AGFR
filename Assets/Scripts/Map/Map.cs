using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/Map/Map")]
public class Map : MonoBehaviour {

	public SpriteSheet sheet;

	public static Vector2 MAPDIM = new Vector2(25,25);
	
	public static Tile emptyTile;
	public static Tile errTile;
	
	private Dictionary<string, Tile> tiles = new Dictionary<string,Tile>();
	public string[,] map;
	private Vector2 dimensions;
	private Vector2 spawn;
	
	private TileData[,] tileData;
	private bool isDirty = false;
	
	public Tile GetTile(string name) { return tiles[name]; }
	public string[,] MapData { get { return map; } }
	public Color[] GetPixelsAt(int x, int y) { return GetTileAt(x,y).Pixels; }
	public Vector2 Dimensions { get { return dimensions; } }
	public Vector2 Spawn { get { return spawn; } }
	public bool IsDirty { get { return isDirty; } }
	
	public void Init(Vector2 dimensions, Vector2 spawn){
		emptyTile = new Tile("empty",0, Color.clear, Color.white, 1, sheet);
		errTile = new Tile("error",0,Color.red, Color.white, 999, sheet);
		map = new string[(int)MAPDIM.x,(int)MAPDIM.y];
		tileData = new TileData[(int)MAPDIM.x,(int)MAPDIM.y];
		foreach (EntityLayer elayer in this.GetComponentsInChildren<EntityLayer>()){
			elayer.Init(MAPDIM);
		}
	}
	
	public Tile GetTileAt(int x, int y) { 
		if(x < 0 || x >= MAPDIM.x) return errTile;
		if(y < 0 || y >= MAPDIM.y) return errTile;
		/*Vector2 offset = new Vector2((int)((MAPDIM.x-this.dimensions.x)/2), //Offset to center
		                             (int)((MAPDIM.y-this.dimensions.y)/2));
		int xo = x - (int)offset.x;
		int yo = y - (int)offset.y;*/
		
		//if(xo < 0 || xo >= Dimensions.x) return emptyTile;
		//if(yo < 0 || yo >= Dimensions.y) return emptyTile;
		string t = map[x,y];
		if(t == null) return emptyTile;
		return tiles[t]; 
	}
	
	public void SetTileAt(int x, int y, string tile){
		map[x,y] = tile;
		this.MarkDirty();
	}
	
	public TileData GetTileDataAt(int x, int y){
		if(tileData[x,y] == null) tileData[x,y] = new TileData(x,y);
		return tileData[x,y];
	}
	
	public void SetTile(string name, Tile t){
		tiles[name] = t;
	}
	
	public void UseTile(Vector2 loc, Entity e){
		Tile t = GetTileAt((int)loc.x, (int)loc.y);
		TileData dat = GetTileDataAt((int)loc.x, (int)loc.y);
		t.OnUse(e, dat);
	}
	
	// Update is called once per frame
	void Update () {
		if(Game.Paused) return;
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
}
