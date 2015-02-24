using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/UI/Inventory UI")]
public class InventoryUI : MonoBehaviour {

	public PremadeContainer items;

	public LayerMask UILayer;
	private InvTile currentlyDragging = null;
	private InvSlot lastSlot = null;
	private InvSlot lastSlotLocation = null;
	
	public GameObject tilePrefab;
	
	public InvSlot[] inventory;
	public InvSlot[] armor;
	public InvSlot[] equipment;
	public Collider mapCol;

	// Use this for initialization
	void Start () {
		SetItem (equipment[0], items["iron dagger"]);
		AddItem (items["green potion"]);
	}
	
	// Update is called once per frame
	void Update () {
		if(lastSlot != null){
			lastSlot.SetEmpty();
			lastSlot = null;
		}
		Camera cam = Camera.main;
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		if(Input.GetMouseButton(0) && currentlyDragging){
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
		this.lastSlotLocation  = tile.Slot;
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
				if(CanPlace(currentlyDragging, slot)){ //Moving to new slot
					if(lastSlotLocation.type == Item.Types.EQUIPMENT || lastSlotLocation.type == Item.Types.ARMOR){
						currentlyDragging.Item.SendMessage("OnUnequip");
					}
					SwapSlots(lastSlotLocation, slot);
					//currentlyDragging.Slot = slot; 
					if(slot.type == Item.Types.EQUIPMENT || slot.type == Item.Types.ARMOR){
						currentlyDragging.Item.SendMessage("OnEquip");
					}
				}else { 
					//Reset Slot
					currentlyDragging.Slot = lastSlotLocation;
				}
			}else{
				//Drop
				currentlyDragging.Slot = lastSlotLocation;
			}
		}else {
			//Reset slot
			currentlyDragging.Slot = lastSlotLocation;
		}
		
		currentlyDragging = null;
		
		//if can place here, place here
		//	reset tile.startSlot
		//else put at tile.startSlot
		
	}
	
	public bool CanPlace(InvTile item, InvSlot slot){
		//Checking swap item
		if(slot.Tile){
			InvTile other = slot.Tile;
			if(other.Type != lastSlotLocation.type && lastSlotLocation.type != Item.Types.GENERIC) return false;
		}
		
		return item.Type == slot.type || slot.type == Item.Types.GENERIC;
	}
	
	public bool isDragging{
		get {return currentlyDragging != null;}
	}
	
	public void AddItem(GameObject itemPrefab){
		if(!HasOpenSlot()) return;
		GameObject item = (GameObject)Instantiate(itemPrefab);
		GameObject add = (GameObject)Instantiate(tilePrefab);
		add.transform.parent = this.transform;
		INamed namer = (INamed)item.GetComponent(typeof(INamed));
		add.GetComponent<InvTile>().Init(this, item, this.inventory[GetNextMatchingSlot(namer.Name())]);
		//add.transform.posinventory[0]
	}
	public void SetItem(InvSlot slot, GameObject itemPrefab){
		GameObject item = (GameObject)Instantiate(itemPrefab);
		GameObject add = (GameObject)Instantiate(tilePrefab);
		add.transform.parent = this.transform;
		INamed namer = (INamed)item.GetComponent(typeof(INamed));
		add.GetComponent<InvTile>().Init(this, item, slot);
		//add.transform.posinventory[0]
	}
	
	public void SwapSlots(InvSlot a, InvSlot b){
		InvTile ta = a.Tile;
		InvTile tb = b.Tile;
		
		if(ta) ta.Slot = b;
		else b.Tile = null;
		
		if(tb) tb.Slot = a;
		else a.Tile = null;
		
	}
	
	public GameObject GetWeapon(){
		return this.equipment[0].item;
	}
	
	public int GetNextMatchingSlot(string itemName=null){
		int firstEmpty = -1;
		for (int i = 0; i < inventory.Length; i++){
			InvSlot slot = inventory[i];
			if(!slot.Tile) {
				if(firstEmpty == -1) firstEmpty = i;
			}else {
				INamed namer = (INamed)slot.Tile.Item.GetComponent(typeof(INamed));
				if(namer.Name() == itemName) 
					return i;
			}
		}
		return firstEmpty;
	}
	
	public bool HasOpenSlot(){
		return this.GetNextMatchingSlot() > -1;
	}
}
