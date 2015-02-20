using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("Scripts/Game/Player/Player Health Bar")]
[RequireComponent(typeof(HealthTracker))]
public class PlayerHealthBar : MonoBehaviour {
	public Color blinkColor = new Color(255,200,180);
	[Range(0,1)]
	public float flashPercent = 0.2f;
	private GameObject healthBar;
	private HealthTracker health;
	
	void Start() {
		this.health = GetComponent<HealthTracker>();
		this.healthBar = GameObject.FindGameObjectWithTag("HealthBar");
	}
	
	// Update is called once per frame
	void Update () {
		float scale = (float)health.Health / (float)health.maxHealth;
		healthBar.transform.localScale = new Vector3(1,scale, 1);
		
		if(scale <= flashPercent){
			GameObject.FindGameObjectWithTag("DyingOverlay").GetComponent<Image>().enabled = true;
		}else if(scale > flashPercent){
			GameObject.FindGameObjectWithTag("DyingOverlay").GetComponent<Image>().enabled = false;
		}
	}
}
