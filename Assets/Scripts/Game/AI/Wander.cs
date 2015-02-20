using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/AI/Wander")]
[RequireComponent(typeof(Entity))]
public class Wander : MonoBehaviour {

	public int delay = 1000;

	private int timer = 0;
	private Entity ent;
	private bool holdStill = false;
	// Use this for initialization
	void Start () {
		ent = this.GetComponent<Entity>();
	}
	
	// Update is called once per frame
	void Update () {
		timer += (int)(Time.deltaTime*1000);
		if(!holdStill && timer >= delay){
			timer = 0;
			int randX = Random.Range(-1,2);
			int randY = Random.Range(-1,2);
			Vector2 move = new Vector2(randX,randY);
			this.ent.Move(move);
		}
	}
	
	public void HoldStill(){
		holdStill = true;
	}
	
	private void ResumeMoving(){
		holdStill = false;
	}
}
