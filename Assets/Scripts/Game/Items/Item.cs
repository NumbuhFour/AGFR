using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Items/Item")]
public class Item : MonoBehaviour, INamed {
	public enum Types { GENERIC, ARMOR, EQUIPMENT }

	public string name;
	public int count = 1;
	
	//Can only one fit in stack?
	public bool singular = true;

	public Types type = Types.GENERIC;
	
	public string Name() { return name; }
	
	void OnUse(){
		
	}
	
	void OnEquip(){ //For both armor and equipment
	
	}
	void OnUnequip(){
	
	}
	
	void OnSwing(string direction){
	
	}
}
