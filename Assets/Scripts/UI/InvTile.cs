using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Item))]
public class InvTile : MonoBehaviour {
	public InventoryUI invMan;
	public SpriteSheet sheet;
	
	private float lockedZ;
	private Vector3 screenPoint;
	private Vector3 offset;
	private InvSlot slot;
	private Item item;
	
	public InvSlot Slot{ 
		get { return slot; } 
		set {
			this.slot = value;
			Vector3 pos = slot.transform.position;
			this.transform.position = new Vector3(pos.x,pos.y,this.transform.position.z);
		}
	}
	
	public void Init(InventoryUI invMan, SpriteSheet sheet, Item item){
		this.invMan = invMan;
		this.sheet = sheet;
		this.item = item;
		this.PaintTexture();
	}
	
	public Item Item{
		get { return item; }
		set { item = value; }
	}
	
	public void PaintTexture(){
		Color[] pixels = sheet.GetPixelData(Item.spriteIndex);
		
		Texture2D tex = new Texture2D(sheet.tileResolution,sheet.tileResolution);
		
		tex.SetPixels(0,0,sheet.tileResolution, sheet.tileResolution, pixels);
		
		tex.filterMode = FilterMode.Point;
		tex.Apply ();
		this.renderer.sharedMaterials[0].mainTexture = tex;
	}
	
	
	void OnMouseDown(){
		invMan.SetCurrentlyDragging(this);
		lockedZ = gameObject.transform.position.z;
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position); // I removed this line to prevent centring 
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		Screen.showCursor = false;
	}
	
	void OnMouseDrag(){
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, lockedZ);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		curPosition.z = lockedZ;
		transform.position = curPosition;
	}
	void OnMouseUp(){
		Screen.showCursor = true;
		invMan.DragStopped();
	}
	
}
