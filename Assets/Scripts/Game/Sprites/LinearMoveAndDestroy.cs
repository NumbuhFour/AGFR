using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Sprites/Linear Move")]
public class LinearMoveAndDestroy : MonoBehaviour {
	
	public int delay = 300;
	
	private int timer = 0;
	private bool holdStill = false;
	private Entity ent;
	private string dir = "";
	// Use this for initialization
	void Start () {
		ent = this.GetComponent<Entity>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Game.Paused) return;
		timer += (int)(GameTime.deltaTime*1000);
		if(!holdStill && timer >= delay){
			timer = 0;
			Vector2 move = Direction.ConvertToVector(dir);
			if(!this.ent.CanMove(move)){
				Destroy(this.gameObject);
			}else
				this.ent.Move(move);
		}
	}
	
	public void HoldStill(){
		holdStill = true;
	}
	
	private void ResumeMoving(){
		holdStill = false;
	}
	
	public void SetDirection(string direction){
		this.dir = direction;
	}
	
	public void OnDamageDealt(Entity target){
		Destroy(this.gameObject);
	}
}
