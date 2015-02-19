using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HealthTracker))]
public class PlayerHealthBar : MonoBehaviour {
	public Color blinkColor = new Color(255,200,180);
	[Range(0,1)]
	public float flashPercent = 0.2f;
	private GameObject healthBar;
	private HealthTracker health;
	
	private Flash flash = null;
	void Start() {
		this.health = GetComponent<HealthTracker>();
		this.healthBar = GameObject.FindGameObjectWithTag("HealthBar");
	}
	
	// Update is called once per frame
	void Update () {
		float scale = (float)health.Health / (float)health.maxHealth;
		healthBar.transform.localScale = new Vector3(1,scale, 1);
		
		if(scale <= flashPercent && flash == null){
			flash = healthBar.AddComponent<Flash>();
			flash.Init(-1,500,200,blinkColor);
		}else if(scale > flashPercent && flash != null){
			Destroy(flash);
			flash = null;
		}
	}
}
