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
	
	[Range(0f,1f)]
	public float darkMinAlpha = 0.5f;
	[Range(0f,1f)]
	public float darkMaxAlpha = 0.7f;
	
	[Range(10f, 2000f)]
	public float lightsOutFrames = 200f;
	
	[Range(0,100)]
	public int darkChangePeriod = 7;

	private float min;
	private float max;
	
	public bool lightsOut = false;
	private float timer = 0;

	void Update(){
		timer += Time.deltaTime*1000f;
		if(((minAlpha != min || maxAlpha != max) && !lightsOut ) || (lightsOut && timer > lightsOutFrames)){
			Regenerate();
			timer = 0;
		}
	}
	public void Regenerate(){
		int tileSize = map.sheet.tileResolution+2;
		Texture2D tex = new Texture2D(tileSize*(int)Map.MAPDIM.x-2, tileSize*(int)Map.MAPDIM.y-2);
		clear (tex);
		min = lightsOut ? darkMinAlpha:minAlpha;
		max = lightsOut ? darkMaxAlpha:maxAlpha;
		int pushSeed = Random.seed;
		for (int x = 0; x < Map.MAPDIM.x; x++){
			for (int y = 0; y < Map.MAPDIM.y; y++){
				if(map.GetTileAt(x,y) != Map.emptyTile){
					Random.seed = x * (int)Map.MAPDIM.x + y;
					Color draw = color;
					if(lightsOut){
						float theta = (Time.time*50)/darkChangePeriod + Random.Range(0,darkChangePeriod);
						float sine = Mathf.Sin(theta)*10;
						draw.r = draw.g = draw.b = 0;
						draw.a = Random.Range(min,max) + sine/255;
					}else{
						draw.a = Random.Range(min,max);
					}
				
					for(int tx=0; tx<tileSize; tx++){
						for(int ty=0; ty<tileSize; ty++){
							tex.SetPixel(x*tileSize+tx,y*tileSize+ty,draw);
						}
					}
				}
				
			}
		}
		Random.seed = pushSeed;
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
	
	public bool LightsOut {
		get { return lightsOut; }
		set { lightsOut = value; }
	}
	
}
