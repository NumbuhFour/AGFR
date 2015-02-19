using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(FaceDirection))]
public class PlayerMovement : MonoBehaviour {

	public int moveDelay = 150;
	public int tapDelay = 50;

	private long lastTime;
	
	private Entity ent;
	private FaceDirection dir;
	private int lastHoriz = 0;
	private int lastVert = 0;
	
	private bool vertMostRecent = false;

	// Use this for initialization
	void Start () {
		ent = GetComponent<Entity>();
		dir = GetComponent<FaceDirection>();
		lastTime = (int)(Time.time*1000);
	}
	
	// Update is called once per frame
	void Update () {
		long time = (int)(Time.time*1000);
		int horiz = (int)Input.GetAxisRaw("Horizontal");
		int vert = (int)Input.GetAxisRaw("Vertical");
		long delta = time-lastTime;
		
		if(horiz != 0 && horiz != lastHoriz) vertMostRecent = false;
		if(vert != 0 && vert != lastVert) vertMostRecent = true;
		
		if(delta > moveDelay || ((lastHoriz != horiz || lastVert != vert) && delta > tapDelay)) {
			
			Vector2 delt = new Vector2();
			
			if(vertMostRecent){ //Changes preference order
				if(vert != 0){
					delt.y = vert;
				}else if(horiz != 0){
					delt.x = horiz;
				}
			}else {
				if(horiz != 0){
					delt.x = horiz;
				}else if(vert != 0){
					delt.y = vert;
				}
			}
			ent.Move (delt);
			SetDirection(delt);
			lastTime = time;
		}
		
		lastHoriz = horiz;
		lastVert = vert;
	}
	
	private void SetDirection(Vector2 delt){
		if(delt.x > 0) dir.SetDirection("east");
		if(delt.x < 0) dir.SetDirection("west");
		if(delt.y > 0) dir.SetDirection("north");
		if(delt.y < 0) dir.SetDirection("south");
	}
}
