using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Face Direction")]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Entity))]
public class FaceDirection : MonoBehaviour {

	[Header("East,North,West,South,Idle")]
	public Sprite[] directions;
	
	public string direction;
	
	public void SetDirection(string direction){
		this.direction = direction;
		int dir = Direction.ConvertToIndex(direction);
		((SpriteRenderer)GetComponent<Renderer>()).sprite = directions[dir];
	}
}
