using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Map : MonoBehaviour {

	public string file;
	public TextAsset blargh;
	public SpriteSheet sheet;
	public Color background = Color.green;//new Color(32f/255f,32f/255f,32f/255f,0f);

	private Vector2 MAPDIM = new Vector2(25,25);
	
	private Tile emptyTile;
	private Tile errTile;
	
	private Dictionary<string, Tile> tiles = new Dictionary<string,Tile>();
	private string[,] map;
	private Vector2 dimensions;
	private Vector2 spawn;
	private JSONNode data;

	public Tile GetTile(string name) { return tiles[name]; }
	public string[,] MapData { get { return map; } }
	public Color[] GetPixelsAt(int x, int y) { return GetTileAt(x,y).Pixels; }
	public Vector2 Dimensions { get { return dimensions; } }
	public Vector2 Spawn { get { return spawn; } }
	
	public Tile GetTileAt(int x, int y) { 
		if(x < 0 || x >= MAPDIM.x) return errTile;
		Vector2 offset = new Vector2((int)((MAPDIM.x-this.dimensions.x)/2),
		                             (int)((MAPDIM.y-this.dimensions.y)/2));
		int xo = x - (int)offset.x;
		int yo = y - (int)offset.y;
		
		if(xo < 0 || xo >= Dimensions.x) return emptyTile;
		if(yo < 0 || yo >= Dimensions.y) return emptyTile;
		return tiles[map[xo,yo]]; 
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
		PopulateTiles(data["tiles"]);
		PopulateMap(data["map"]);
		RepaintMap();
	}
	
	private void PopulateMap(JSONNode mapData){
		for(int x = 0; x < dimensions.x; x++){
			for(int y = 0; y < dimensions.y; y++){
				map[x,y] = mapData[y][x];
			}
		}
	}
	
	private void PopulateTiles(JSONNode tileData){
		int count = tileData.Count;
		for (int i = 0; i < count; i++){
			JSONNode t = tileData[i];
			tiles[t["name"]] = new Tile(t, sheet);
		}
	}
	
	public void RepaintMap(){
		Texture2D tex = new Texture2D((sheet.tileResolution+2)*(int)MAPDIM.x-2, (sheet.tileResolution+2)*(int)MAPDIM.y-2);
		
		Vector2 offset = new Vector2((int)((MAPDIM.x-this.dimensions.x)/2),
									 (int)((MAPDIM.y-this.dimensions.y)/2));
		
		for(int x = 0; x<25*18; x++){
			for(int y = 0; y<25*18; y++){
				tex.SetPixel(x,y,background);
			}
		}
		for(int x = 0; x<MAPDIM.x; x++){
			for(int y = 0; y<MAPDIM.y; y++){
				//int xo = (int)(x+offset.x);
				//int yo = (int)(y+offset.y);
				Color[] pixels = GetPixelsAt(x,y);
				tex.SetPixels(x*18,y*18,sheet.tileResolution, sheet.tileResolution, pixels);
			}
		}
		
		tex.filterMode = FilterMode.Point;
		tex.Apply ();
		this.renderer.sharedMaterials[0].mainTexture = tex;
	}
	
	
	// Update is called once per frame
	void Update () {
	
	}
}
