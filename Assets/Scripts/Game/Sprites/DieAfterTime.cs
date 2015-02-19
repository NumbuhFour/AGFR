using UnityEngine;
using System.Collections;

public class DieAfterTime : MonoBehaviour {

	public int duration = 400;
	
	private int timer = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += (int)(Time.deltaTime*1000);
		if(timer >= duration){
			Destroy(this.gameObject);
		}
	}
}
