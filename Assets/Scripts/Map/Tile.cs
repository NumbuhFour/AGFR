using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

public class Tile {
	
	private SpriteSheet sheet;
	
	public string name;
	private int spriteIndex;
	private Color mainColor;
	private Color swapColor;
	private int solidity;
	private bool hasLight;
	
	private Color[] pixelData;
	private Color[] modifiedPixelData;
	
	public string Name { get { return name; } }
	public int Sprite { get { return spriteIndex; } }
	public Color MainColor { get { return mainColor; } }
	public Color SwapColor { get { return swapColor; } }
	public int Solidity { get { return solidity; } }
	public bool HasLight { get { return hasLight; } }
	public Color[] Pixels { get { return modifiedPixelData; } }
	public Color[] UnmodifiedPixels { get { return pixelData; } }
	
	public Tile(JSONNode data, SpriteSheet sheet){
		JSONNode mcNode = data["main_color"];
		Color mc = MapLoader.ReadColor(mcNode);
		JSONNode sNode = data["swap_color"];
		Color sc = MapLoader.ReadColor(sNode);
		
		Init (data["name"].Value, data["sprite"].AsInt, mc,sc, data["solidity"].AsInt, sheet);
	}
	
	public Tile(string name, int spriteIndex, Color mainColor, Color swapColor, int solidity, bool hasLight, SpriteSheet sheet){
		Init (name,spriteIndex,mainColor,swapColor,solidity, hasLight,sheet);
	}
	
	public void Init(string name, int spriteIndex, Color mainColor, Color swapColor, int solidity, bool hasLight, SpriteSheet sheet){
		this.name = name;
		this.spriteIndex = spriteIndex;
		this.mainColor = mainColor;
		this.swapColor = swapColor;
		this.solidity = solidity;
		this.sheet = sheet;
		
		this.pixelData = sheet.GetPixelData(spriteIndex);
		this.modifiedPixelData = sheet.GetPixelData(spriteIndex);
		
		PerformColorSwap(modifiedPixelData, swapColor, mainColor);
	}
	
	private void PerformColorSwap(Color[] data, Color key, Color result){
		for(int i = 0; i < data.Length; i++){
			if(data[i] == key)
				data[i] = result;
		}
	}
	
	public virtual void OnEntityEnter(Entity e, TileData data){
	}
	public virtual void OnEntityExit(Entity e, TileData data){
	}
	public virtual void OnEntityAttack(Entity e, TileData data){
	}
	public virtual void Update(TileData data){
	}
	public virtual void OnUse(Entity e, TileData data){
	}
	
	public virtual void ReadData(JSONNode node, TileData data){
	}
	
}
