using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Test/AddSomeRandoText")]
public class AddSomeRandoText : MonoBehaviour {

	public ChatManager chat;
	public string[,] text = {{"[<color=red>Cheese</color>]: ", "Yargen FlargenM!"},{"[Taters]: ", "Muchas Gravy rawrawra awrawrasdas awrasdasdasdas adasda adsdada asdadad ad ad adaasd!"},{"[Tatasaers]: ", "asfas Gsdfasdfravy!"}};
	public int delay = 0;
	private long lastTime;
	private int done = 1;

	// Use this for initialization
	void Start () {
		lastTime = (long)(GameTime.time*1000);
		chat.PushText(text[0,0], text[0,1]);
	}
	
	// Update is called once per frame
	void Update () {
		long time = (long)(GameTime.time*1000);
		if(time - lastTime >= delay && done < 3){
			lastTime = (long)(GameTime.time*1000);
			chat.PushText(text[done,0], text[done,1]);
			done++;
		}
	}
}
