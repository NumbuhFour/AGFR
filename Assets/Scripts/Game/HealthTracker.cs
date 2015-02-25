using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Health Tracker")]
public class HealthTracker : MonoBehaviour {

	public int maxHealth = 10;
	public int invulnDuration = 1500;
	
	private int health;
	private bool invincible = false;
	private Flash damFlash;
	
	public int Health { get { return health; } }
	
	void Start(){
		health = maxHealth;
	}
	
	public void TakeDamage(int amt, Entity dealer){
		if(invincible){
			//InvinFlash ();
			return;
		}
		if(maxHealth <= 0) return;
		health -= amt;
		health = Mathf.Max(health,0);
		if(health <= 0){
			this.SendMessage("Death");
			return;
		}
		FlashDamage();
		this.invincible = true;
	}
	
	public void Heal(int amt){
		health = Mathf.Min(health+amt,maxHealth);
	}
	
	private void FlashDamage(){
		if(damFlash) {
			damFlash.Reset();
		}
		
		damFlash = this.gameObject.AddComponent<Flash>();
		damFlash.Init(invulnDuration, 200,150, Color.red, "EndInvinPhase");
	}
	
	private void InvinFlash(){
		Flash add = this.gameObject.AddComponent<Flash>();
		add.Init(100, 100, 0, Color.cyan);//Single flash
	}
	public void EndInvinPhase(){
		this.invincible = false;
		damFlash = null;
	}
}
