using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Test/AddSomeRandoText")]
public class AddSomeRandoText : MonoBehaviour {

	public ChatManager chat;
	public string[,] text = {{"[<color=red>Cheese</color>]: ", "Hello world!"},{"[Taters]: ", "(Press [USE] to continue) "}};
	public int delay = 0;
	private long lastTime;
	private int done = 1;

	// Use this for initialization
	void Start () {
		lastTime = (long)(GameTime.time*1000);
		chat.PushText(text[0,0], text[0,1],true);
	}
	
	// Update is called once per frame
	void Update () {
		long time = (long)(GameTime.time*1000);
		if(time - lastTime >= delay && done < text.Length/2){
			lastTime = (long)(GameTime.time*1000);
			chat.PushText(text[done,0], text[done,1],true);
			done++;
		}
	}
}
