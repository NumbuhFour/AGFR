using UnityEngine;
using System.Collections;

public class SpawnSpriteOnSwing : MonoBehaviour {

	public PremadeContainer sprites;
	public string spriteName;

	private EntityLayer spriteLayer;
	private Entity player;
	
	void Start(){
		spriteLayer = GameObject.FindGameObjectWithTag("SpriteLayer").GetComponent<EntityLayer>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
	}

	void OnSwing(string direction){
		Vector2 dir = ConvertDirection(direction);
		Entity e = spriteLayer.SpawnEntity(sprites[spriteName], player.loc + dir).GetComponent<Entity>();
		e.GetComponent<FaceDirection>().SetDirection(direction);
	}
	
	private Vector2 ConvertDirection(string dir){
		switch(dir.ToLower()){
		case "east": return new Vector2(1,0);
		case "west": return new Vector2(-1,0);
		case "north": return new Vector2(0,1);
		case "south": return new Vector2(0,-1);
		}
		return new Vector2(0,-1);
	}
}
