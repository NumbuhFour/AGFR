using UnityEngine;
using System.Collections;

public class AddSomeRandoText : MonoBehaviour {

	public ChatManager chat;
	public string[,] text = {{"[<color=red>Cheese</color>]: ", "Yargen <color=red>FlargenM</color>!"},{"[Taters]: ", "Muchas Gravy!"},{"[Tatasaers]: ", "asfas Gsdfasdfravy!"}};
	public int delay = 3000;
	private long lastTime;
	private int done = 1;

	// Use this for initialization
	void Start () {
		lastTime = (long)(Time.time*1000);
		chat.PushText(text[0,0], text[0,1]);
	}
	
	// Update is called once per frame
	void Update () {
		long time = (long)(Time.time*1000);
		if(time - lastTime >= delay && done < text.Length-1){
			lastTime = (long)(Time.time*1000);
			chat.PushText(text[done,0], text[done,1]);
			done++;
		}
	}
}
