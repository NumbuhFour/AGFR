using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Sprites/Hit Damage")]
[RequireComponent(typeof(Entity))]
public class HitDamage : MonoBehaviour {

	public int damage = 1;
	public int knockback = 1;
	
	private Entity sprite;
	private EntityLayer entLayer;
	private Entity owner;
	
	private bool hit = false;
	// Use this for initialization
	void Start () {
		sprite = this.GetComponent<Entity>();
		entLayer = GameObject.FindGameObjectWithTag("EntityLayer").GetComponent<EntityLayer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!hit){
			Entity target = entLayer.GetEntityAt(sprite.loc);
			if(target && target != owner){
				hit = true;
				HealthTracker health = target.GetComponent<HealthTracker>();
				if(!health) return;
				health.TakeDamage(damage,sprite);
				if(knockback > 0){
					FaceDirection fd = gameObject.GetComponent<FaceDirection>();
					if(fd)
						target.Knock(fd.direction, 1);
				}
			}
		}
	}
	
	void SetOwner(Entity e){ //Called by SendMessage, sets creator of the swing
		owner = e;
	}
}
