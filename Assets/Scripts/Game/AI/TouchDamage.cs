using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/AI/Touch Damage")]
[RequireComponent(typeof(Entity))]
public class TouchDamage : MonoBehaviour {
	public int delayBeforeFirstHit = 0;
	public int delayBetweenHits = 100;
	public int strength = 1;
	
	private int timer;
	private bool initalHit = false;
	private bool hasCompany = false;
	private Entity ent;
	// Use this for initialization
	void Start () {
		ent = GetComponent<Entity>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Game.Paused) return;
		bool lastCompany = hasCompany;
		hasCompany = CheckNeighbors();
		if(hasCompany){
			this.gameObject.SendMessage("HoldStill");
			if(lastCompany){
				timer += (int)(GameTime.deltaTime * 1000);
				
				if(!initalHit){
					if(timer >= delayBeforeFirstHit){
						Swing();
						initalHit = true;
						timer = 0;
					}
				}else{
					if(timer >= delayBetweenHits){
						Swing();
						timer = 0;
					}
				}
				
			}else{
				timer = 0;
				initalHit = false;
			}
		}else if(lastCompany){
			this.gameObject.SendMessage("ResumeMoving");
		}
	}
	
	void Swing(){
		Vector2 loc = ent.loc;
		for(int x = -1; x <= 1; x++)
		for(int y = -1; y <= 1; y++) {
			Entity get = ent.entlayer.GetEntityAt(loc + new Vector2(x,y));
			if(get != null && get.gameObject.tag == "Player"){
				HealthTracker h = get.GetComponent<HealthTracker>();
				h.TakeDamage(strength, this.ent);
			}
		}
	}
	
	bool CheckNeighbors(){
		Vector2 loc = ent.loc;
		for(int x = -1; x <= 1; x++)
			for(int y = -1; y <= 1; y++) {
			Entity get = ent.entlayer.GetEntityAt(loc + new Vector2(x,y));
				if(get != null && get.gameObject.tag == "Player"){
					return true;
				}
			}
		return false;
	}
}
