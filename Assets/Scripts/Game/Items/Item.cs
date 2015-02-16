using UnityEngine;
using System.Collections;

public class Item {

	public SpriteSheet sheet;
	public int spriteIndex;
	public enum Types { GENERIC, ARMOR, EQUIPMENT }

	public Types type = Types.GENERIC;
	
	public Item (int sprite, SpriteSheet sheet, Types type) {
		this.spriteIndex = sprite;
		this.sheet = sheet;
		this.type = type;
	}
	
	void OnUse(){
		
	}
	
	void OnEquip(){ //For both armor and equipment
	
	}
	void OnUnequip(){
	
	}
}
