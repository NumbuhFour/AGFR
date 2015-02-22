using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game")]
public class Game : MonoBehaviour {

	private static Game instance;
	public static Game Instance { get { return instance; } }

	private bool paused;
	public static bool Paused { 
		get { return Instance.paused; }
		set {
			bool change = Instance.paused != value;
			Instance.paused = value;
			
		}
	}
		

	// Use this for initialization
	void Start () {
		instance = this;
	}
}
