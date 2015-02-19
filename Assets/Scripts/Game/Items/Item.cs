using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour, INamed {
	public enum Types { GENERIC, ARMOR, EQUIPMENT }

	public string name;
	public int count = 1;

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
