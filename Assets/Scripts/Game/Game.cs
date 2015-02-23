using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game")]
public class Game : MonoBehaviour {
	
	public GameObject pauseOverlay;
	
	private static Game instance;
	public static Game Instance { get { return instance; } }

	private bool paused;
	private float pauseDuration  = 0;
	public static bool Paused { 
		get { return Instance.paused; }
		set {
			bool change = Instance.paused != value;
			Instance.paused = value;
			Instance.pauseOverlay.SetActive(Instance.paused);
			if(change && Instance.paused) Instance.pauseDuration = 0;
			if(change && !Instance.paused) GameTime.AddPauseOffset(Instance.pauseDuration);
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
