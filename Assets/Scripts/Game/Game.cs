using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game")]
public class Game : MonoBehaviour {
	public string startingTestLevelName;
	
	public GameObject pauseOverlay;
	
	private static Game instance;
	public static Game Instance { get { return instance; } }

	private SaveData saveData;

	private bool paused;
	private bool chatPause;
	private float pauseDuration  = 0;
	private float chatPauseDuration  = 0;
	
	private Vector2 spawn = new Vector2(-1, -1);
	
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
	
	public static SaveData Save{ get { return Instance.saveData; } }
		

	// Use this for initialization
	void Start () {
		if(Game.instance){
			this.saveData = this.saveData;
		}else{
			this.saveData = new SaveData();
		}
		Game.instance = this;
		
		LoadLevel (startingTestLevelName);
	}
	
	void Update(){
		if(Paused) pauseDuration += Time.deltaTime;
	}
	
	public static void LoadLevel(string file){
		Debug.Log("Loading level " + file);
		TextAsset level = (TextAsset) Resources.LoadAssetAtPath<TextAsset>("Assets/Resources/Maps/" + file + ".json");
		if(level == null) Debug.Log ("MAP FILE NOT FOUND NOOOO");
		BroadcastToAll ("OnLevelReset");
		Instance.GetComponent<MapLoader>().Load(level);
	}
	public static Vector2 LevelSpawn{
		get { return Instance.spawn; } 
		set { Instance.spawn = value; }
	}
	
	public static void BroadcastToAll(string func, object options=null) {
		GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
		foreach (GameObject go in gos) {
			go.SendMessage(func, options, SendMessageOptions.DontRequireReceiver);
		}
	}
}
