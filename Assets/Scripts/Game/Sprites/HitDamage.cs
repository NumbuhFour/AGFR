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
	private FaceDirection fd;
	
	private bool hit = false;
	private bool corner = false;
	private Vector3 cornerCalc;
	// Use this for initialization
	void Start () {
		sprite = this.GetComponent<Entity>();
		entLayer = GameObject.FindGameObjectWithTag("EntityLayer").GetComponent<EntityLayer>();
		fd = gameObject.GetComponent<FaceDirection>();
		
		
		bool flipped = false;
		FlipSpriteByLastDirection flippy = GetComponent<FlipSpriteByLastDirection>();
		if(flippy){
			flipped = flippy.IsFlipped;
		}
		int dir = Direction.ConvertToIndex(fd.direction);
		Vector3 findLastSwing = this.sprite.loc + Direction.ConvertToVector(dir+2)/* Back */ + Direction.ConvertToVector(dir+ (flipped?-1:1));
		cornerCalc = this.sprite.loc + Direction.ConvertToVector(dir+ (flipped?-1:1));
		
		Entity otherSwing = sprite.entlayer.GetEntityAt(findLastSwing);
		if(otherSwing && otherSwing.Name() == sprite.Name() && otherSwing.GetComponent<HitDamage>().owner == this.owner){ //Sword was found adjacent-ish
			corner = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//if(!hit){
		Entity target = entLayer.GetEntityAt(sprite.loc);
		if(target && target != owner){
			hit = true;
			HealthTracker health = target.GetComponent<HealthTracker>();
			if(!health) return;
			health.TakeDamage(damage,sprite);
			if(knockback > 0){
				if(fd)
					target.Knock(fd.direction, 1);
			}
		}
		if(corner){
			Entity targetCorner = entLayer.GetEntityAt(cornerCalc);
			if(targetCorner && targetCorner != owner){
				hit = true;
				HealthTracker health = targetCorner.GetComponent<HealthTracker>();
				if(!health) return;
				health.TakeDamage(damage,sprite);
				if(knockback > 0){
					if(fd)
						targetCorner.Knock(fd.direction, 1);
				}
			}
		}
		//}
	}
	
	void SetOwner(Entity e){ //Called by SendMessage, sets creator of the swing
		owner = e;
	}
}
