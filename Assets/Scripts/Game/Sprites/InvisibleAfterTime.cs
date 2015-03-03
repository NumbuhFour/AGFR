using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Sprites/Invisible After Time")]
public class InvisibleAfterTime : MonoBehaviour {
	
	public int duration = 400;
	
	private int timer = 0;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timer += (int)(GameTime.deltaTime*1000);
		if(timer >= duration){
			this.gameObject.GetComponent<Renderer>().enabled = false;
			Destroy (this);
		}
	}
}