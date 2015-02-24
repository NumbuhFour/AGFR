using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Test/AddSomeRandoText")]
public class AddSomeRandoText : MonoBehaviour {

	public ChatManager chat;
	public string[,] text = {{"[<color=red>Cheese</color>]: ", "Hello world!"},{"", "(Press [USE] to continue) "}};
	public int delay = 0;
	private long lastTime;
	private int done = 1;

	// Use this for initialization
	void Start () {
		lastTime = (long)(GameTime.time*1000);
		chat.PushText(text[0,0], text[0,1],ChatManager.PauseMode.PAUSE_GAME,this);
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void OnFinishText(){
		chat.PushText(text[1,0], text[1,1],ChatManager.PauseMode.STOP_AFTER_FINISH);
	}
}
