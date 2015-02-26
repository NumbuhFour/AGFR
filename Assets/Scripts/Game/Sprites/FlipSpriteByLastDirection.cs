using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Sprites/Flip Sprite By Last Direction")]
public class FlipSpriteByLastDirection : MonoBehaviour {

	
	private string lastDirection;
	private Vector3 scale = new Vector3(1f,1f,1f);
	private bool isFlipped = false;
	
	void Start(){
	}
	
	void SetLastDirection(string direction){
		lastDirection = direction;
	}
	
	void SetDirection(string direction){
		float flip = 1;
		if(lastDirection != ""){
			switch(direction.ToLower()){
			case "north":
			{
				if(lastDirection == "east")
					flip = -1;
				this.transform.localScale = new Vector3(scale.x * flip, scale.y, scale.z);
				break;
			}
			case "south":
			{
				if(lastDirection == "west") flip = -1;
				this.transform.localScale = new Vector3(scale.x * flip, scale.y, scale.z);
				break;
			}
			case "east":
			{
				if(lastDirection == "south") flip = -1;
				this.transform.localScale = new Vector3(scale.x, scale.y * flip, scale.z);
				break;
			}
			case "west":
			{
				if(lastDirection == "north") flip = -1;
				this.transform.localScale = new Vector3(scale.x, scale.y * flip, scale.z);
				break;
			}
			}
		}
		isFlipped = flip==-1;
		this.lastDirection = direction;
		
	}
	
	public bool IsFlipped { get { return this.isFlipped; } }
}
