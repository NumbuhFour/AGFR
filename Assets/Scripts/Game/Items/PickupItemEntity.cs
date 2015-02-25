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
		int pickedup = count-rem;
		if(pickedup != 0) 
			GameObject.FindGameObjectWithTag("Chat").GetComponent<ChatManager>().PushText("", "Picked up " + ((pickedup==1)?"a":pickedup.ToString()) + " " + item.GetComponent<Item>().DisplayName());
		if(rem <= 0) {
			Destroy(gameObject);
		}else{
			GameObject.FindGameObjectWithTag("Chat").GetComponent<ChatManager>().PushText("", "Inventory full!");
			this.count = rem;
		}
		//Destroy(gameObject);
	}
}
