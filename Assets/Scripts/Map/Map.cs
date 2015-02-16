using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Map : MonoBehaviour {

	public string file;
	public TextAsset blargh;
	public SpriteSheet sheet;
	public Color background = Color.green;//new Color(32f/255f,32f/255f,32f/255f,0f);
	public Transform entityContainer;

	public static Vector2 MAPDIM = new Vector2(25,25);
	
	public static Tile emptyTile;
	public static Tile errTile;
	
	private Dictionary<string, Tile> tiles = new Dictionary<string,Tile>();
	private string[,] map;
	private Vector2 dimensions;
	private Vector2 spawn;
	private JSONNode data;
	
	private TileData[,] tileData;

	public Tile GetTile(string name) { return tiles[name]; }
	public string[,] MapData { get { return map; } }
	public Color[] GetPixelsAt(int x, int y) { return GetTileAt(x,y).Pixels; }
	public Vector2 Dimensions { get { return dimensions; } }
	public Vector2 Spawn { get { return spawn; } }
	
	public Tile GetTileAt(int x, int y) { 
		if(x < 0 || x >= MAPDIM.x) return errTile;
		if(y < 0 || y >= MAPDIM.y) return errTile;
		Vector2 offset = new Vector2((int)((MAPDIM.x-this.dimensions.x)/2), //Offset to center
		                             (int)((MAPDIM.y-this.dimensions.y)/2));
		int xo = x - (int)offset.x;
		int yo = y - (int)offset.y;
		
		if(xo < 0 || xo >= Dimensions.x) return emptyTile;
		if(yo < 0 || yo >= Dimensions.y) return emptyTile;
		return tiles[map[xo,yo]]; 
	}
	
	public void SetTileAt(int x, int y, string tile){
		map[x,y] = tile;
		RepaintMap();
	}
	
	public TileData GetTileDataAt(int x, int y){
		if(tileData[x,y] == null) tileData[x,y] = new TileData(x,y);
		return tileData[x,y];
	}

	// Use this for initialization
	void Start () {
		emptyTile = new Tile("empty",0,background, Color.white, 1, sheet);
		errTile = new Tile("error",0,Color.red, Color.white, 999, sheet);
		//TextAsset input = Resources.Load("Maps/" + file) as TextAsset;
		data = JSON.Parse(blargh.text);
		ParseData();
	}
	
	private void ParseData(){
		dimensions = new Vector2(data["info"]["width"].AsInt, data["info"]["height"].AsInt);
		spawn = new Vector2(data["info"]["spawn"][0].AsInt, data["info"]["spawn"][1].AsInt);
		map = new string[(int)dimensions.x,(int)dimensions.y];
		tileData = new TileData[(int)MAPDIM.x,(int)MAPDIM.y];
		PopulateTiles(data["tiles"]);
		PopulateMap(data["map"]);
	}
	
	private void PopulateMap(JSONNode mapData){
		for(int x = 0; x < dimensions.x; x++){
			for(int y = 0; y < dimensions.y; y++){
				map[x,y] = mapData[y][x];
			}
		}
	}
	
	private void PopulateTiles(JSONNode tileJData){
		int count = tileJData.Count;
		for (int i = 0; i < count; i++){
			JSONNode t = tileJData[i];
			tiles[t["name"]] = new Tile(t, sheet);
		}
	}
	
	public void RepaintMap(){
		int tileSize = sheet.tileResolution+2;
		Texture2D tex = new Texture2D(tileSize*(int)MAPDIM.x-2, tileSize*(int)MAPDIM.y-2);
		
		for(int x = 0; x<MAPDIM.x*tileSize; x++){ //Clear
			for(int y = 0; y<MAPDIM.y*tileSize; y++){
				tex.SetPixel(x,y,background);
			}
		}
		for(int x = 0; x<MAPDIM.x; x++){
			for(int y = 0; y<MAPDIM.y; y++){
				Color[] pixels = GetPixelsAt(x,y);
				tex.SetPixels(x*tileSize,y*tileSize,sheet.tileResolution, sheet.tileResolution, pixels);
				Tile t = GetTileAt(x,y);
				if(t != errTile && t != emptyTile){
					TileData td = GetTileDataAt(x,y);
					object get = td["highlight"];
					if(get == null) continue;
					Color highlight = (Color)get;
					if(highlight != Color.clear){
						int max = this.sheet.tileResolution+1;
						for(int hx = -1; hx <= max; hx++){
							for(int hy = -1; hy <= max; hy++){
								if(hx == -1 || hy == -1 || hx == max || hy == max) //Only edges
								   tex.SetPixel(x*tileSize+hx,y*tileSize+hy,highlight);
							}
						}
					}
				}
			}
		}
		
		tex.filterMode = FilterMode.Point;
		tex.Apply ();
		this.renderer.sharedMaterials[0].mainTexture = tex;
	}
	
	public void NotifyMove(Entity e, Vector2 to, Vector2 from){
		Tile toTile = GetTileAt((int)to.x, (int)to.y);
		TileData toData = GetTileDataAt((int)to.x,(int)to.y);
		Tile fromTile = GetTileAt((int)from.x, (int)from.y);
		TileData fromData = GetTileDataAt((int)from.x,(int)from.y);
		
		fromTile.OnEntityExit(e,fromData);
		toTile.OnEntityEnter(e,toData);
	}
	
	// Update is called once per frame
	void Update () {
		for(int x = 0; x<MAPDIM.x; x++){
			for(int y = 0; y<MAPDIM.y; y++){
				Tile t = GetTileAt (x,y);
				if(t == errTile || t == emptyTile) continue;
				
				TileData data = GetTileDataAt(x,y);
				t.Update(data);
			}
		}
	}
	
	void LateUpdate(){
		RepaintMap();
	}
}
