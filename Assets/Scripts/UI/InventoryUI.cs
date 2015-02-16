using UnityEngine;
using System.Collections;

public class InventoryUI : MonoBehaviour {

	public SpriteSheet sheet;
	public LayerMask UILayer;
	private InvTile currentlyDragging = null;
	private InvSlot lastSlot = null;
	
	public GameObject tilePrefab;
	
	public InvSlot[] inventory;
	public InvSlot[] armor;
	public InvSlot[] equipment;
	public Collider mapCol;

	// Use this for initialization
	void Start () {
		AddItem (new Item(0,sheet,Item.Types.GENERIC));
	}
	
	// Update is called once per frame
	void Update () {
		if(lastSlot != null){
			lastSlot.SetEmpty();
			lastSlot = null;
		}
		Camera cam = Camera.main;
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		if(Input.GetMouseButton(0)){
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit,1000,UILayer)){
				GameObject hitGo = hit.collider.gameObject;
				if(hitGo.tag == "InvSlot"){
					lastSlot = hitGo.GetComponent<InvSlot>();
					if(CanPlace(currentlyDragging, lastSlot)) lastSlot.SetSelected();
					else lastSlot.SetError();
				}
			}
		}
	}
	
	public void SetCurrentlyDragging(InvTile tile){
		this.currentlyDragging = tile;
	}
	
	public void DragStopped(){
	
		InvSlot slot;
		Camera cam = Camera.main;
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit,1000,UILayer)){
			GameObject hitGo = hit.collider.gameObject;
			if(hitGo.tag == "InvSlot"){
				slot = hitGo.GetComponent<InvSlot>();
				if(CanPlace(currentlyDragging, slot)){
					currentlyDragging.Slot = slot;
				}else {
					currentlyDragging.Slot = currentlyDragging.Slot;
				}
			}else{
				//Drop
				currentlyDragging.Slot = currentlyDragging.Slot;
			}
		}else {
			currentlyDragging.Slot = currentlyDragging.Slot;
		}
		
		currentlyDragging = null;
		
		//if can place here, place here
		//	reset tile.startSlot
		//else put at tile.startSlot
		
	}
	
	public bool CanPlace(InvTile item, InvSlot slot){
		return item.Item.type == slot.type || slot.type == Item.Types.GENERIC;
	}
	
	public bool isDragging{
		get {return currentlyDragging != null;}
	}
	
	public void AddItem(Item item){
		GameObject add = (GameObject)Instantiate(tilePrefab);
		add.transform.parent = this.transform;
		add.GetComponent<InvTile>().Init(this,this.sheet, item);
		//add.transform.posinventory[0]
	}
}
