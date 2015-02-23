using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game")]
public class Game : MonoBehaviour {
	
	public GameObject pauseOverlay;
	
	private static Game instance;
	public static Game Instance { get { return instance; } }

	private bool paused;
	private bool chatPause;
	private float pauseDuration  = 0;
	private float chatPauseDuration  = 0;
	public static bool Paused { //When game is paused
		get { return Instance.paused; } 
		set {
			bool change = Instance.paused != value;
			Instance.paused = value;
			Instance.pauseOverlay.SetActive(Instance.paused);
			if(change && Instance.paused) Instance.pauseDuration = 0;
			if(change && !Instance.paused) GameTime.AddPauseOffset(Instance.pauseDuration);
		}
	}
	
	public static void NoMenuPauseGame(){
		bool change = !Instance.paused;
		Instance.paused = true;
		if(change && Instance.paused) Instance.pauseDuration = 0;
	}
	
	public static bool ChatPause { //When chat text is paused 
		get { return Instance.chatPause; } 
		set {
			bool change = Instance.chatPause != value;
			Instance.chatPause = value;
			if(change && Instance.chatPause) Instance.pauseDuration = 0;
			if(change && !Instance.chatPause) GameTime.AddPauseOffset(Instance.pauseDuration);
		}
	}
		

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	void Update(){
		if(Paused) pauseDuration += Time.deltaTime;
	}
}
