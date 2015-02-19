using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Item))]
public class InvTile : MonoBehaviour {
	public InventoryUI invMan;
	
	private float lockedZ;
	private Vector3 screenPoint;
	private Vector3 offset;
	private InvSlot slot;
	private GameObject item;
	
	public InvSlot Slot{ 
		get { return slot; } 
		set {
			this.slot = value;
			value.Tile = this;
			Vector3 pos = slot.transform.position;
			this.transform.position = new Vector3(pos.x,pos.y,this.transform.position.z);
		}
	}
	
	public void Init(InventoryUI invMan, GameObject item, InvSlot slot){
		this.invMan = invMan;
		this.item = item;
		item.transform.parent = this.transform;
		item.transform.localPosition = Vector3.zero;
		this.Slot = slot;
	}
	
	public GameObject Item{
		get { return item; }
		set { item = value; }
	}
	
	public Item ItemStack{
		get { return (Item)item.GetComponent(typeof(Item)); }
	}
	public Item.Types Type {
		get { return ItemStack.type; }
	}
	
	void Update(){
		if(Input.GetMouseButtonUp(1)){
			item.SendMessage("OnUse");
		}
	}
	
	
	void OnMouseDown(){
		invMan.SetCurrentlyDragging(this);
		Item.GetComponent<SpriteRenderer>().sortingOrder = 1000; //Bring to front;
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
		Item.GetComponent<SpriteRenderer>().sortingOrder = 1; //Bring to front;
	}
	
}
