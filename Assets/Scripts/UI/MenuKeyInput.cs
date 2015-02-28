using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/UI/Menu Key Input")]
public class MenuKeyInput : MonoBehaviour {

	public bool pauseReleased = true;
	
	// Update is called once per frame
	void Update () {
		float pause = Input.GetAxisRaw("Pause");
		if(pause > 0 && pauseReleased){
			Game.Paused = !Game.Paused;
			Game.ChatPause = Game.Paused;
			pauseReleased = false;
		}
		
		if(pause <= 0) pauseReleased = true;
	}
}
