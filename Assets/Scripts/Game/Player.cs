using UnityEngine;
using System.Collections;

public class Player : Entity {

	public int moveDelay = 50;

	private long lastTime;

	// Use this for initialization
	public override void Start () {
		lastTime = (int)(Time.time*1000);
	}
	
	// Update is called once per frame
	public override void Update () {
		long time = (int)(Time.time*1000);
		if(time - lastTime > moveDelay) {
			int horiz = (int)Input.GetAxisRaw("Horizontal");
			int vert = (int)Input.GetAxisRaw("Vertical");
			
			Vector2 delt = new Vector2(horiz,vert);
			if(delt.sqrMagnitude > 0){
				Move (delt);
				lastTime = time;
			}
		}
	}
}
