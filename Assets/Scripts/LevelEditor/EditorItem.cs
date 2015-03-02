using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/LevelEditor/Item")]
public class EditorItem {
	public enum Types { TILE, ENTITY }
	
	public Types itemType;

	private string name;
	private string type;
	private int spriteID;
	private Color mainColor;
	private Color swapColor;
	private int solidity;
	
	private Tile tile;
	
	//private string entityName;
	
	public EditorItem(string name, string type, int spriteID, Color mainColor, Color swapColor, int solidity){
		this.itemType = Types.TILE;
		this.name = name;
		this.type = type;
		this.spriteID = spriteID;
		this.mainColor = mainColor;
		this.swapColor = swapColor;
		this.solidity = solidity;
	}
	public string Name {
		get { return this.name; }
		set { this.name = value; RefreshTile(); }
	}
	
	public string Type {
		get { return this.type; }
		set { this.type = value; RefreshTile(); }
	}
	
	public int SpriteID {
		get { return this.spriteID; }
		set { this.spriteID = value; RefreshTile(); }
	}
	
	public Color MainColor {
		get { return this.mainColor; }
		set { this.mainColor = value; RefreshTile(); }
	}
	
	public Color SwapColor {
		get { return this.swapColor; }
		set { this.swapColor = value; RefreshTile(); }
	}
	
	public int Solidity {
		get { return this.solidity; }
		set { this.solidity = value; RefreshTile(); }
	}
	
	public Tile Tile {
		get {
			if(itemType == Types.TILE && tile == null){
				tile = new Tile(name, spriteID, mainColor, swapColor, solidity,Game.GameObject.GetComponent<SpriteSheet>());
			}
			return tile;
		}
	}
	
	private void RefreshTile(){
		if(tile != null){
			tile.Init(name, spriteID, mainColor, swapColor, solidity,Game.GameObject.GetComponent<SpriteSheet>());
		}
	}
}
