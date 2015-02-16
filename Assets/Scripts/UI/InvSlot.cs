using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class InvSlot : MonoBehaviour {

	public InventoryUI invMan;
	public Sprite selectedSprite;
	public Sprite errSprite;
	public Sprite fullSprite = null;
	
	public Item.Types type = Item.Types.GENERIC;
	
	private Sprite normalSprite;
	private SpriteRenderer ren;
	private bool selected = false;
	
	private InvTile tile;
	
	void Start(){
		ren = GetComponent<SpriteRenderer>();
		normalSprite = ren.sprite;
		
	}

	/*void OnMouseEnter	(){ CheckSelected(true); }
	void OnMouseExit	(){ CheckSelected(false); }
	void OnMouseDown	(){ CheckSelected(true); }
	void OnMouseDrag	(){ CheckSelected(true); }
	void OnMouseUp		(){ CheckSelected(false); }
	
	private void CheckSelected(bool on){
		if(invMan.isDragging){
			selected = true;
			ren.sprite = selectedSprite;
		}else{
			if(selected){
				invMan.DragStopped(this);
			}
			selected = false;
			ren.sprite = normalSprite;
		}
	}*/
	
	public void SetFull(){
		if(fullSprite != null) ren.sprite = fullSprite;
		else ren.sprite = normalSprite;
	}
	public void SetEmpty(){
		ren.sprite = normalSprite;
	}
	
	public void SetSelected(){
		ren.sprite = selectedSprite;
	}
	
	public void SetError(){
		ren.sprite = errSprite;
	}
	
	public InvTile Tile{
		get { return tile; }
		set {
			this.tile = value;
			Vector3 pos = tile.transform.position;
			Vector3 mPos = this.transform.position;
			tile.transform.position = new Vector3(mPos.x, mPos.y, pos.z);
		}
	}
	
	public Item item{
		get{ 
			if(Tile == null) return null;
			return Tile.Item; 
		}
	}
}
