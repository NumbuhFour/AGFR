using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Player/Player Movement")]
[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(FaceDirection))]
public class PlayerMovement : MonoBehaviour {

	public int moveDelay = 150;
	public int tapDelay = 50;
	
	private long lastTime;
	private long lastTimeHold;
	
	private Entity ent;
	private Map map;
	private FaceDirection dir;
	private static int lastHoriz = 0; //Static to keep across levels
	private static int lastVert = 0;
	
	private bool vertMostRecent = false;

	public void OnLevelLoaded(){
		ent = GetComponent<Entity>();
		dir = GetComponent<FaceDirection>();
		map = ent.map;
		lastTime = (int)(GameTime.time*1000) + tapDelay; //Adding tapdelay to both as its a little more responsive
		//lastTimeHold = (int)(GameTime.time*1000) + tapDelay;
	}
	
	// Update is called once per frame
	void Update () {
		if(Game.Paused) return;
		long time = (int)(GameTime.time*1000);
		int horiz = (int)Input.GetAxisRaw("Horizontal");
		int vert = (int)Input.GetAxisRaw("Vertical");
		long delta = time-lastTime;
		//long deltaHold= time-lastTimeHold;
		bool change = ((horiz!=0 && lastHoriz != horiz) || (lastVert != vert && vert!=0));
		if(horiz != 0 || vert != 0){
			if(horiz != 0 && horiz != lastHoriz) vertMostRecent = false;
			if(vert != 0 && vert != lastVert) vertMostRecent = true;
			
			if((!change && delta > moveDelay) || (change && delta > tapDelay)) {
				Vector2 delt = new Vector2();
				
				if(vertMostRecent){ //Changes preference order based on last key pressed
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
				
				if(ent.CanMove(delt)){
					map.CenterCameraOn(ent.loc+delt);
					ent.Move (delt);
				}
				SetDirection(delt);
				lastTime = time;
				//lastTimeHold = time;
			}
			//if(change || (horiz==0 && vert==0)) lastTimeHold = time; //For when tapping makes it think its held
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
