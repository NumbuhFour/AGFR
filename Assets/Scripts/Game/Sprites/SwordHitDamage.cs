using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Sprites/Sword Hit Damage")]
[RequireComponent(typeof(Entity))]
public class SwordHitDamage : HitDamage {
	
	private bool corner = false;
	private Vector3 cornerCalc;
	// Use this for initialization
	public override void Start () {
		base.Start();
		
		bool flipped = false;
		FlipSpriteByLastDirection flippy = GetComponent<FlipSpriteByLastDirection>();
		if(flippy){
			flipped = flippy.IsFlipped;
		}
		int dir = Direction.ConvertToIndex(fd.direction);
		Vector3 findLastSwing = this.sprite.loc + Direction.ConvertToVector(dir+2)/* Back */ + Direction.ConvertToVector(dir+ (flipped?-1:1));
		cornerCalc = this.sprite.loc + Direction.ConvertToVector(dir+ (flipped?-1:1));
		
		Entity otherSwing = sprite.entlayer.GetEntityAt(findLastSwing);
		if(otherSwing && otherSwing.Name() == sprite.Name() && otherSwing.GetComponent<HitDamage>().Owner == this.Owner){ //Sword was found adjacent-ish
			corner = true;
		}
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
		if(corner){
			Entity targetCorner = entLayer.GetEntityAt(cornerCalc);
			if(targetCorner && targetCorner != owner){
				hit = true;
				HealthTracker health = targetCorner.GetComponent<HealthTracker>();
				if(!health) return;
				health.TakeDamage(damage,sprite);
				this.SendMessage("OnDamageDealt",targetCorner,SendMessageOptions.DontRequireReceiver);
				if(knockback > 0){
					if(fd)
						targetCorner.Knock(fd.direction, 1);
				}
			}
		}
	}
}
