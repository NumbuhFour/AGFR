using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Map/Light Noise")]
public class LightNoise : MonoBehaviour {

	public Map map;

	public Color color;

	[Range(0f,1f)]
	public float minAlpha = 0.2f;
	[Range(0f,1f)]
	public float maxAlpha = 0.5f;

	private float min;
	private float max;

	void Update(){
		if(minAlpha != min || maxAlpha != max) Regenerate();
	}
	public void Regenerate(){
		int tileSize = map.sheet.tileResolution+2;
		Texture2D tex = new Texture2D(tileSize*(int)Map.MAPDIM.x-2, tileSize*(int)Map.MAPDIM.y-2);
		clear (tex);
		min = minAlpha;
		max = maxAlpha;
		for (int x = 0; x < Map.MAPDIM.x; x++){
			for (int y = 0; y < Map.MAPDIM.y; y++){
				if(map.GetTileAt(x,y) != Map.emptyTile){
					Color draw = color;
					draw.a = Random.Range(min,max);
				
					for(int tx=0; tx<tileSize; tx++){
						for(int ty=0; ty<tileSize; ty++){
							tex.SetPixel(x*tileSize+tx,y*tileSize+ty,draw);
						}
					}
				}
				
			}
		}
		tex.filterMode = FilterMode.Point;
		tex.Apply ();
		this.renderer.sharedMaterials[0].mainTexture = tex;
	}
	
	private void clear(Texture2D tex){
		int tileSize = map.sheet.tileResolution+2;
		for (int x = 0; x < Map.MAPDIM.x*tileSize; x++){
			for (int y = 0; y < Map.MAPDIM.y*tileSize; y++){
				tex.SetPixel(x,y,Color.clear);
				
			}
		}
	}
	
}
