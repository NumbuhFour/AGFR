using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Items/Pickup Item")]
public class PickupItemEntity : MonoBehaviour {
	
	public GameObject item;
	public int count = 1;
	
	public void SetItem(GameObject item){
		this.item = item;
	}
	public void SetItemCount(int count){
		this.count = count;
	}

	public void OnUse(Entity e){
		InventoryUI inv = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryUI>();
		int rem = inv.AddItem(item, count);
		if(rem <= 0) {
			Destroy(gameObject);
		}else{
			this.count = rem;
		}
		//Destroy(gameObject);
	}
}
