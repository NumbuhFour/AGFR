using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/Map/Map")]
public class Map : MonoBehaviour {

	public SpriteSheet sheet;

	public static Vector2 MAPDIM = new Vector2(25,25);
	
	public string mapName = ""; //really only used for editor filename
	
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
	
	public Dictionary<string,Tile> Tiles{ get { return tiles; } }
	public string[,] MapArray{ get { return map; } }
	
	//private Vector2 offset;
	//public Vector2 Offset { get { return offset; } }
	
	private Vector2 camLoc;
	public Vector2 CamLoc{ 
		get { return camLoc; } 
		set { 
			Vector2 last = new Vector2(camLoc.x, camLoc.y);
			int border = 0;
			if(Game.Mode == Game.GameMode.LEVEL_EDITOR) border = 1;
			if(this.dimensions.x >= MAPDIM.x) {//Only scroll if scrollable
				camLoc.x = value.x;
				camLoc.x = Mathf.Max(camLoc.x, -border);
				camLoc.x = Mathf.Min(camLoc.x, (dimensions.x - MAPDIM.x) + border);
				camLoc.x = Mathf.Floor(camLoc.x);
				if(camLoc.x != last.x) MarkDirty();
			}
			if(this.dimensions.y >= MAPDIM.y){
				camLoc.y = value.y;
				camLoc.y = Mathf.Max(camLoc.y, -border);
				camLoc.y = Mathf.Min(camLoc.y, (dimensions.y - MAPDIM.y) + border);
				camLoc.y = Mathf.Floor(camLoc.y);
				if(camLoc.y != last.y) MarkDirty();
			}
			
			if(camLoc != last){
				//tell entity layers to update
				this.gameObject.BroadcastMessage("CameraMove", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	public void Init(Vector2 dimensions){
		sheet = Game.GameObject.GetComponent<SpriteSheet>();
		if(emptyTile == null){
			emptyTile = new Tile("empty",0, Color.clear, Color.white, 1, false,  sheet);
			errTile = new Tile("error",0,Color.red, Color.white, 999, false, sheet);
		}
		
		this.dimensions = dimensions;
		camLoc = new Vector2(-(int)((MAPDIM.x-this.dimensions.x)/2), //Offset to center
		                     -(int)((MAPDIM.y-this.dimensions.y)/2));
		tiles = new Dictionary<string,Tile>();
		map = new string[(int)dimensions.x,(int)dimensions.y];
		tileData = new TileData[(int)dimensions.x,(int)dimensions.y];
		foreach (EntityLayer elayer in this.GetComponentsInChildren<EntityLayer>()){
			elayer.Init(dimensions);
		}
	}
	
	public void CenterCameraOn(Vector2 pos){
		CamLoc = (pos - (MAPDIM/2) + new Vector2(1,1));
	}
	
	public Tile GetTileAt(int x, int y) { 
		if(!IsLocValid(x,y)) return errTile;
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
		if(tile != null) tile = tile.Trim();
		if(!IsLocValid(x,y)) return;
		string old = map[x,y];
		Tile oTile;
		if(old != null && (oTile = tiles[old]) != null) oTile.OnRemoved(GetTileDataAt(x,y));
		tileData[x,y] = null;
		map[x,y] = tile;
		if(tile != null){
			Tile nTile = tiles[tile];
			if(nTile != null) nTile.OnPlaced(GetTileDataAt(x,y));
		}
		this.MarkDirty();
	}
	/// Sets within the scale of the map's dimensions, not max dimensions
	public void SetTileAndTileDataAt(int x, int y, string tile, TileData data){
		tile = tile.Trim();
		if(!IsLocValid(x,y)) return;
		string old = map[x,y];
		Tile oTile;
		if(old != null && (oTile = tiles[old]) != null) oTile.OnRemoved(GetTileDataAt(x,y));
		tileData[x,y] = data;
		map[x,y] = tile;
		Tile nTile = tiles[tile];
		if(nTile != null) nTile.OnPlaced(GetTileDataAt(x,y));
		this.MarkDirty();
	}
	
	public TileData GetTileDataAt(int x, int y){
		if(!IsLocValid(x,y)) return null;
		if(tileData[x,y] == null){
			Tile t = GetTileAt(x,y);
			if(t != null) tileData[x,y] = t.GetDefaultTileData(x,y);
			else tileData[x,y] = new TileData(x,y);
		}
		return tileData[x,y];
	}
	
	public TileData GetNewTileDataAt(int x, int y){
		if(!IsLocValid(x,y)) return null;
		return new TileData(x,y);
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
		
		if(this.IsDirty){
			this.gameObject.BroadcastMessage("OnRepaintMap", SendMessageOptions.RequireReceiver);
			this.MarkClean();
		}
		
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
	
	public bool IsLocVisible(Vector2 loc){
		return (loc.x >= camLoc.x && loc.x < camLoc.x + MAPDIM.x) && (loc.y >= camLoc.y && loc.y < camLoc.y + MAPDIM.y);
	}
		
	public bool IsLocValid(Vector2 loc){
		return IsLocValid((int)loc.x, (int)loc.y);
	}
	public bool IsLocValid(int x, int y){
		return (x >= 0 && x < dimensions.x && y >= 0 && y < dimensions.y);
	}
	
	public Vector2 ConvertSceneToWorld(Vector2 pos){
		return pos + camLoc;
	}
	
	public Vector2 ConvertWorldToScene(Vector2 pos){
		return pos - camLoc;
	}
}
