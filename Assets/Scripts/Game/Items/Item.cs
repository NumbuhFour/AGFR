using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Items/Item")]
public class Item : MonoBehaviour, INamed {
	public enum Types { GENERIC, ARMOR, EQUIPMENT }

	public string name;
	public int maxCount;
	private int count = 1;
	
	//Can only one fit in stack?
	public bool singular = true;

	public Types type = Types.GENERIC;
	
	public string Name() { return name; }
	
	public int Count { 
		get { return this.count; }
		set { 
			this.count = value; 
			if(transform.parent) transform.parent.SendMessage("OnItemCountChange");
		}
	}
	
	void OnUse(){
		
	}
	
	void OnEquip(){ //For both armor and equipment
	
	}
	void OnUnequip(){
	
	}
	
	void OnSwing(string direction){
	
	}
}
