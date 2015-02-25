using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Entity/Drop Item On Death With Chance")]
public class DropItemOnDeathWithChance : MonoBehaviour {

	public GameObject lootsackPrefab;
	public GameObject item;
	
	[Range(0,100)]
	public float chance = 50;
	
	public int minAmountDropped = 1;
	public int maxAmountDropped = 1;
	
	public void Death(){
		Entity ent = gameObject.GetComponent<Entity>();
		GameObject spawn = ent.entlayer.SpawnEntity(lootsackPrefab, ent.loc);
		spawn.SendMessage("SetItem", item);
		spawn.SendMessage("SetItemCount", Random.Range(minAmountDropped, maxAmountDropped+1));
	}
}
