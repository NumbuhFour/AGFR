using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Sprites/Hit Damage")]
[RequireComponent(typeof(Entity))]
public class HitDamage : MonoBehaviour {

	public int damage = 1;
	public int knockback = 1;
	
	protected Entity sprite;
	protected EntityLayer entLayer;
	protected Entity owner;
	protected FaceDirection fd;
	protected bool hit;
	// Use this for initialization
	public virtual void Start () {
		sprite = this.GetComponent<Entity>();
		entLayer = GameObject.FindGameObjectWithTag("EntityLayer").GetComponent<EntityLayer>();
		fd = gameObject.GetComponent<FaceDirection>();
	}
	
	// Update is called once per frame
	public virtual void Update () {
		//if(!hit){
		Entity target = entLayer.GetEntityAt(sprite.loc);
		if(target && target != owner){
			hit = true;
			HealthTracker health = target.GetComponent<HealthTracker>();
			if(!health) return;
			health.TakeDamage(damage,sprite);
			this.SendMessage("OnDamageDealt",target,SendMessageOptions.DontRequireReceiver);
			if(knockback > 0){
				if(fd)
					target.Knock(fd.direction, 1);
			}
		}
	}
	
	void SetOwner(Entity e){ //Called by SendMessage, sets creator of the swing
		owner = e;
	}
	
	public Entity Owner{ get { return owner; } }
	
	public void SetDamage(int damage){
		this.damage = damage;
	}
}
