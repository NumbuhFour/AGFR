using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[AddComponentMenu("Scripts/LevelEditor/Editor Inventory Slot")]
[RequireComponent(typeof(BoxCollider))]
public class EditorInvSlot : MonoBehaviour {

	public EditorUI invMan;
	
	public GameObject ren;
	
	public EditorItem.Types type = EditorItem.Types.TILE;
	
	private EditorItem item;
	
	public EditorItem Item{
		get{ 
			return item; 
		}
		set {
			item = value;
			Render ();
		}
	}
	private Texture2D tileTex;
	private SpriteSheet sheet;
	
	public void Init(EditorUI invMan, EditorItem item){
		this.invMan = invMan;
		this.item = item;
		
		bool type = (item.itemType == EditorItem.Types.TILE);
		if(type) { //Tile
			sheet = Game.GameObject.GetComponent<SpriteSheet>();
			if(!tileTex) {
				tileTex = new Texture2D(sheet.tileResolution, sheet.tileResolution);
				//this.tileRenderer.renderer.material.mainTexture = tileTex;
				tileTex.filterMode = FilterMode.Point;
			}
		}
		Render ();
	}
	
	public void OnClick(){
		if(Input.GetKey("left shift")) {
			invMan.AddTilePresetFromUser(this.Item);
		} else
			invMan.SetTool(this.Item);
	}
	
	public void Update(){
		if(item != null) Render(); 
	}
		
	
	void Render(){
		if(tileTex == null){
			sheet = Game.GameObject.GetComponent<SpriteSheet>();
			tileTex = new Texture2D(sheet.tileResolution, sheet.tileResolution);
			//this.tileRenderer.renderer.material.mainTexture = tileTex;
			tileTex.filterMode = FilterMode.Point;
		}
	
		if(item != null && item.itemType == EditorItem.Types.TILE){
			Color[] pixels = item.Tile.Pixels;
			tileTex.SetPixels(pixels);
			tileTex.Apply ();
			ren.GetComponent<Image>().sprite = 
				Sprite.Create(tileTex, new Rect(0,0,16,16), new Vector2(0.5f,0.5f), 1f);
		}else if(item == null){
			ClearTex();
		}
	}
	
	
	private static Color[] clearPx = null;
	private void ClearTex(){
		if(clearPx == null){
			int dim = this.sheet.tileResolution*this.sheet.tileResolution;
			clearPx = new Color[dim];
			for(int i = 0; i < dim; i++)
				clearPx[i] = Color.clear;
		}
		tileTex.SetPixels(clearPx);
		tileTex.Apply();
	}
}
