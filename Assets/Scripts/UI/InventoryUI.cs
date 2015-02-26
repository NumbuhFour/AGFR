using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[AddComponentMenu("Scripts/UI/Inventory UI")]
public class InventoryUI : MonoBehaviour {

	public PremadeContainer items;

	public LayerMask UILayer;
	public Canvas itemCountCanvas;
	public GameObject itemCountPrefab;
	
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
		if(Input.GetMouseButtonUp(1)){
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit,1000,UILayer)){
				GameObject hitGo = hit.collider.gameObject;
				if(hitGo && hitGo.tag == "InvSlot"){
					GameObject item = hitGo.GetComponent<InvSlot>().item;
					if(item) item.SendMessage("OnUse");
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
	
	//Returns amount remaining after merge with inventory
	public int AddItem(GameObject itemPrefab, int count=1){
		if(!HasOpenSlot()) return -1;
		//Making item object
		GameObject item = (GameObject)Instantiate(itemPrefab);
		GameObject add = (GameObject)Instantiate(tilePrefab);
		GameObject counter = (GameObject)Instantiate(itemCountPrefab);
		add.transform.SetParent (this.transform);
		counter.transform.SetParent(this.itemCountCanvas.transform);
		
		//Placing in inventory
		Item itemstack = item.GetComponent<Item>();
		itemstack.Count = count;
		int putSlot = GetNextMatchingSlot(itemstack, true);
		if(putSlot == -1){ //Merged or inv full
			count = itemstack.Count;
			Destroy (item);
			Destroy (add);
			Destroy (counter);
			return count;
		}else {
			add.GetComponent<InvTile>().Init(this, item, this.inventory[putSlot], counter); //Empty slot, place there
			return 0;
		}
		//add.transform.posinventory[0]
	}
	public void SetItem(InvSlot slot, GameObject itemPrefab){
		GameObject item = (GameObject)Instantiate(itemPrefab);
		GameObject add = (GameObject)Instantiate(tilePrefab);
		GameObject counter = (GameObject)Instantiate(itemCountPrefab);
		add.transform.SetParent (this.transform);
		counter.transform.SetParent(this.itemCountCanvas.transform);
		INamed namer = (INamed)item.GetComponent(typeof(INamed));
		add.GetComponent<InvTile>().Init(this, item, slot, counter);
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
	
	public int GetNextMatchingSlot(Item item=null, bool merge=true){
		int firstEmpty = -1;
		for (int i = 0; i < inventory.Length; i++){
			InvSlot slot = inventory[i];
			if(!slot.Tile) {
				if(item == null) return i;
				if(firstEmpty == -1) firstEmpty = i;
			}else if(item != null) {
				Item found = slot.Tile.Item.GetComponent<Item>();
				if(found.Name() == item.Name()) {
					if(merge){
						int rem = found.maxCount - found.Count;
						found.Count = Mathf.Min (item.Count + found.Count, found.maxCount);
						item.Count = Mathf.Max(0, item.Count - rem);
						if(item.Count <= 0){
							return -1; //Merged completely, no more item
						}
					}else {
						return i; //Slot found matching item
					}
				}
			}
		}
		return firstEmpty; //First empty slot
	}
	
	public bool HasOpenSlot(){
		return this.GetNextMatchingSlot() > -1;
	}
}
