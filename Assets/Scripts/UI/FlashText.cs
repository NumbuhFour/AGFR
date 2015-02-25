using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[AddComponentMenu("Scripts/UI/Flash Text Color")]
public class FlashText : MonoBehaviour {
	
	public int period = 333; //How long to be new color
	public int pause = 100; //How long to be old color
	public Color blinkColor = Color.red;
	
	private int timer = 0;
	private Color orig;
	private Text text;
	
	public void Init(int period, int pause, Color blinkColor){
		this.period = period;
		this.pause = pause;
		this.blinkColor = blinkColor;
	}
	
	// Use this for initialization
	void Start () {
		text = this.GetComponent<Text>();
		orig = text.color;
	}
	
	// Update is called once per frame
	void Update () {
		if(!text) return;
		timer += (int)(Time.deltaTime*1000);
		
		if(timer%(period+pause) > pause){
			text.color = blinkColor;
		}else{
			text.color = orig;
		}
	}
	public void Reset(){
		if(!text) return;
		timer = 0;
		text.color = orig;
	}
	
	public void OnDestroy(){
		if(!text) return;
		text.color = orig;
	}
	
	public void Stop(){
		if(!text) return;
		text.color = orig;
		this.enabled = false;
	}
}
