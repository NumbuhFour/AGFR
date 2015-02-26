using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Sprites/Flip Sprite By Last Direction")]
public class FlipSpriteByLastDirection : MonoBehaviour {

	
	private string lastDirection;
	private Vector3 scale = new Vector3(1f,1f,1f);
	
	void Start(){
	}
	
	void SetLastDirection(string direction){
		lastDirection = direction;
	}
	
	void SetDirection(string direction){
		if(lastDirection != ""){
			switch(direction.ToLower()){
			case "north":
			{
				float flip = 1;
				if(lastDirection == "east") flip = -1;
				this.transform.localScale = new Vector3(scale.x * flip, scale.y, scale.z);
				break;
			}
			case "south":
			{
				float flip = 1;
				if(lastDirection == "west") flip = -1;
				this.transform.localScale = new Vector3(scale.x * flip, scale.y, scale.z);
				break;
			}
			case "east":
			{
				float flip = 1;
				if(lastDirection == "south") flip = -1;
				this.transform.localScale = new Vector3(scale.x, scale.y * flip, scale.z);
				break;
			}
			case "west":
			{
				float flip = 1;
				if(lastDirection == "north") flip = -1;
				this.transform.localScale = new Vector3(scale.x, scale.y * flip, scale.z);
				break;
			}
			}
		}
		this.lastDirection = direction;
		
	}
}
