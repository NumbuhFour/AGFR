using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Map/Map Render")]
public class MapRender : MonoBehaviour {
	
	public Color backgroundColor = new Color(32f/255f,32f/255f,32f/255f,1f);
	public Map map;
	
	private Texture2D tex;
	private int tileRes;
	// Use this for initialization
	void Start () {
		tileRes = Game.GameObject.GetComponent<SpriteSheet>().tileResolution;
		int tileSize = tileRes+2;
		tex = new Texture2D(tileSize*(int)Map.MAPDIM.x, tileSize*(int)Map.MAPDIM.y);
		this.GetComponent<Renderer>().material.mainTexture = tex;
		tex.filterMode = FilterMode.Point;
		tex.wrapMode = TextureWrapMode.Clamp;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(map && map.IsDirty){
			RepaintMap();
			map.MarkClean();
		}
	}
	
	public void RepaintMap(){
		int tileSize = map.sheet.tileResolution+2;
		
		for(int x = 0; x< Map.MAPDIM.x*tileSize; x++){ //Clear
			for(int y = 0; y< Map.MAPDIM.y*tileSize; y++){
				tex.SetPixel(x,y,this.backgroundColor);
			}
		}
		
		if(Game.Mode == Game.GameMode.LEVEL_EDITOR){  //Alignment line thingies
			int step = 7;
			Vector2 offset = this.map.CamLoc;
			offset.x -= ((int)this.map.Dimensions.x/2) %step;
			offset.y -= ((int)this.map.Dimensions.y/2) %step;
			offset.x %= step;
			offset.y %= step;
			float maxX = Map.MAPDIM.x*tileSize;
			float maxY = Map.MAPDIM.y*tileSize;
			for(int x = 0; x <= Map.MAPDIM.x+step; x+=step){
				int xo = (x-(int)offset.x)*tileSize + 8;
				if(xo > 0 && xo < maxX) //Should never be 0. It should be in the center of a tile
				for(int y = 0; y < maxY; y++){
					tex.SetPixel(xo-2, y, Color.red);
					tex.SetPixel(xo-1, y, Color.red);
					tex.SetPixel(xo, y, Color.red);
					tex.SetPixel(xo+1, y, Color.red);
					tex.SetPixel(xo+2, y, Color.red);
					tex.SetPixel(xo+3, y, Color.red);
				}
			}
			for(int y = 0; y <= Map.MAPDIM.y+step; y+=step){
				int yo = (y-(int)offset.y)*tileSize + 8;
				if(yo > 0 && yo < maxY) //Should never be 0. It should be in the center of a tile
				for(int x = 0; x < maxX; x++){
					tex.SetPixel(x, yo-2, Color.red);
					tex.SetPixel(x, yo-1, Color.red);
					tex.SetPixel(x, yo, Color.red);
					tex.SetPixel(x, yo+1, Color.red);
					tex.SetPixel(x, yo+2, Color.red);
					tex.SetPixel(x, yo+3, Color.red);
				}
			}
		}
		
 		for(int x = 0; x< Map.MAPDIM.x; x++){
			for(int y = 0; y< Map.MAPDIM.y; y++){
				Tile t = map.GetVisibleTileAt(x,y);
				if(t != Map.errTile && t != Map.emptyTile){
					Color[] pixels = map.GetPixelsAt(x,y);
					tex.SetPixels(x*tileSize+1,y*tileSize+1,tileRes, tileRes, pixels);
					TileData td = map.GetTileDataVisibleAt(x,y);
					object get = td["highlight"];
					if(get == null) continue;
					Color highlight = (Color)get;
					if(highlight != Color.clear){
						int max = map.sheet.tileResolution+2;
						for(int hx = -1; hx <= max; hx++){
							for(int hy = -1; hy <= max; hy++){
								if(hx == -1 || hy == -1 || hx == max || hy == max) //Only edges
									tex.SetPixel(x*tileSize+hx,y*tileSize+hy,highlight);
							}
						}
					}
				}
				
				if(t == Map.emptyTile && Game.Mode == Game.GameMode.LEVEL_EDITOR && map.IsScenePosWithinMap(new Vector2(x,y))){ //Making empty squares for drawing
					int max = map.sheet.tileResolution;
					for(int hx = 0; hx <= max; hx++){
						for(int hy = 0; hy <= max; hy++){
							tex.SetPixel(x*tileSize+hx,y*tileSize+hy,Color.clear);
						}
					}
				}
			}
		}
		
		tex.Apply ();
	}
}
