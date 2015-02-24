using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Items/Spawn Sprite On Swing")]
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
		Vector2 dir = Direction.ConvertToVector(direction);
		Entity e = spriteLayer.SpawnEntity(sprites[spriteName], player.loc + dir).GetComponent<Entity>();
		e.GetComponent<FaceDirection>().SetDirection(direction);
		//try{
		e.SendMessage("SetOwner", player);
		//}catch(UnityException e){} //No sendmessage reciever
	}
}
