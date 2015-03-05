using UnityEngine;
using System.Collections;
using SimpleJSON;

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
	private bool isLit = true;
	
	private Tile tile;
	
	//private string entityName;
	
	public EditorItem(string name, string type, int spriteID, Color mainColor, Color swapColor, int solidity){
		this.itemType = Types.TILE;
		this.name = name;
		if(type == null || type == "") type = "tile";
		this.type = type;
		this.spriteID = spriteID;
		this.mainColor = mainColor;
		this.swapColor = swapColor;
		this.solidity = solidity;
	}
	
	public EditorItem(EditorItem copy): this(copy.name, copy.type, copy.spriteID, copy.mainColor, copy.swapColor, copy.solidity) {
	}
	
	public EditorItem(Tile t): this(t.Name, t.Type, t.Sprite, t.MainColor, t.SwapColor,t.Solidity){
		this.isLit = t.HasLight;
		this.tile = t;
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
				tile = new Tile(name, spriteID, mainColor, swapColor, solidity, isLit, Game.GameObject.GetComponent<SpriteSheet>());
			}
			return tile;
		}
	}
	
	private void RefreshTile(){
		if(tile != null){
			tile.Init(name, spriteID, mainColor, swapColor, solidity, isLit, Game.GameObject.GetComponent<SpriteSheet>());
		}
	}
	
	public void SaveToJSON(JSONNode json){
		JSONNode clazz = new JSONClass();
		clazz.Add("name", new JSONData(this.name));
		clazz.Add ("type", new JSONData(this.type));
		
		clazz.Add("sprite", new JSONData(this.spriteID));
		clazz.Add ("solidity", new JSONData(this.solidity));
		clazz.Add ("is_lit", new JSONData(this.isLit));
		
		JSONNode mc = new JSONClass();
		mc.Add("r", new JSONData(this.mainColor.r));
		mc.Add("g", new JSONData(this.mainColor.g));
		mc.Add("b", new JSONData(this.mainColor.b));
		mc.Add("a", new JSONData(this.mainColor.a));
		clazz.Add ("main_color", mc);
		
		JSONNode sc = new JSONClass();
		sc.Add("r", new JSONData(this.swapColor.r));
		sc.Add("g", new JSONData(this.swapColor.g));
		sc.Add("b", new JSONData(this.swapColor.b));
		sc.Add("a", new JSONData(this.swapColor.a));
		clazz.Add ("swap_color", sc);
		
		json.Add(clazz);
	}
}
