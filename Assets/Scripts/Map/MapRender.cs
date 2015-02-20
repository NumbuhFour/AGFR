using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Map/Map Render")]
public class MapRender : MonoBehaviour {
	
	public Color backgroundColor = new Color(32f/255f,32f/255f,32f/255f,1f);
	public Map map;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(map.IsDirty){
			RepaintMap();
			map.MarkClean();
		}
	}
	
	public void RepaintMap(){
		int tileSize = map.sheet.tileResolution+2;
		Texture2D tex = new Texture2D(tileSize*(int)Map.MAPDIM.x-2, tileSize*(int)Map.MAPDIM.y-2);
		
		for(int x = 0; x< Map.MAPDIM.x*tileSize; x++){ //Clear
			for(int y = 0; y< Map.MAPDIM.y*tileSize; y++){
				tex.SetPixel(x,y,this.backgroundColor);
			}
		}
 		for(int x = 0; x< Map.MAPDIM.x; x++){
			for(int y = 0; y< Map.MAPDIM.y; y++){
				Tile t = map.GetTileAt(x,y);
				if(t != Map.errTile && t != Map.emptyTile){
					Color[] pixels = map.GetPixelsAt(x,y);
					tex.SetPixels(x*tileSize,y*tileSize,map.sheet.tileResolution, map.sheet.tileResolution, pixels);
					TileData td = map.GetTileDataAt(x,y);
					object get = td["highlight"];
					if(get == null) continue;
					Color highlight = (Color)get;
					if(highlight != Color.clear){
						int max = map.sheet.tileResolution+1;
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
}
