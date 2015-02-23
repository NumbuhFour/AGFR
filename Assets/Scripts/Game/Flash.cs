using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Flash Color")]
public class Flash : MonoBehaviour {

	public int duration;
	public int period = 333; //How long to be new color
	public int pause = 100; //How long to be old color
	public string onDestroy = "";
	public Color blinkColor = Color.red;

	public string id = "";
	
	public string colorKey = "Swap";

	private int timer = 0;
	private Color orig;
	private SpriteRenderer rend;

	public void Init(int duration, int period, int pause, Color blinkColor, string onDestroy = ""){
		this.duration = duration;
		this.period = period;
		this.pause = pause;
		this.blinkColor = blinkColor;
		this.onDestroy = onDestroy;
	}

	// Use this for initialization
	void Start () {
		rend = this.GetComponent<SpriteRenderer>();
		orig = rend.material.GetColor("_Swap");
	}
	
	// Update is called once per frame
	void Update () {
		timer += (int)(GameTime.deltaTime*1000);
		if(duration != -1 && timer > duration){
			rend.material.SetColor ("_Swap",orig);
			if(onDestroy != "") this.gameObject.SendMessage(onDestroy);
			Destroy(this);
			return;
		}
		
		if(timer%(period+pause) < period){
			rend.material.SetColor ("_Swap",blinkColor);
		}else{
			rend.material.SetColor ("_Swap",orig);
		}
	}
	public void Reset(){
		timer = 0;
		rend.material.SetColor ("_Swap",orig);
	}
	
	public void OnDestroy(){
		rend.material.SetColor ("_Swap",orig);
	}
}
