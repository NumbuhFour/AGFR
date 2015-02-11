using UnityEngine;
using System.Collections;
using SimpleJSON;
public class SpriteSheet : MonoBehaviour {
	
	public Texture2D sheet;
	public int tileResolution = 16;

	// Use this for initialization
	void Start () {
		Color[][] tiles = ChopUpTiles();
		Texture2D tex = new Texture2D((tileResolution+2)*25-2, (tileResolution+2)*25-2);
		//tex.SetPixels(tiles[0]);
		
		Color grid = new Color(0f,1f,0f,0.0f);
		
		for(int x = 0; x<25; x++){
			for(int y = 0; y<25*18; y++){
				tex.SetPixel((x+1)*18-1,y,grid);
				tex.SetPixel((x+1)*18-2,y,grid);
			}
		}
		for(int y = 0; y<25; y++){
			for(int x = 0; x<25*18; x++){
				tex.SetPixel(x,(y+1)*18-1,grid);
				tex.SetPixel(x,(y+1)*18-2,grid);
			}
		}
		
		tex.filterMode = FilterMode.Point;
		tex.Apply ();
		Debug.Log("RAWR " + tiles[0][0] + " " + tiles[0][5] + " " + tiles[0][2]);
		this.renderer.sharedMaterials[0].mainTexture = tex;
	}
	
	
	Color[][] ChopUpTiles() {
		int numTilesPerRow = sheet.width / tileResolution;
		int numRows = sheet.height / tileResolution;
		
		Color[][] tiles = new Color[numTilesPerRow*numRows][];
		
		for(int y=0; y<numRows; y++) {
			for(int x=0; x<numTilesPerRow; x++) {
				tiles[y*numTilesPerRow + x] = sheet.GetPixels( x*tileResolution , y*tileResolution, tileResolution, tileResolution );
			}
		}
		
		return tiles;
	}
	
	/*void BuildTexture() {
		DTileMap map = new DTileMap(size_x, size_z);
		
		int texWidth = size_x * tileResolution;
		int texHeight = size_z * tileResolution;
		Texture2D texture = new Texture2D(texWidth, texHeight);
		
		Color[][] tiles = ChopUpTiles();
		
		for(int y=0; y < size_z; y++) {
			for(int x=0; x < size_x; x++) {
				Color[] p = tiles[ map.GetTileAt(x,y) ];
				texture.SetPixels(x*tileResolution, y*tileResolution, tileResolution, tileResolution, p);
			}
		}
		
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		mesh_renderer.sharedMaterials[0].mainTexture = texture;
		
		Debug.Log ("Done Texture!");
	}*/
	
	// Update is called once per frame
	void Update () {
	
	}
}
