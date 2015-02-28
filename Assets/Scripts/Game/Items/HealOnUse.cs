using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Items/Heal On Use")]
public class HealOnUse : MonoBehaviour {
	
	public int amount = 5;

	void OnUse(){
		Game.Player.GetComponent<HealthTracker>().Heal(amount);
		GameObject.FindGameObjectWithTag("Chat").GetComponent<ChatManager>().PushText("", "Healed " + amount + " hp!");
		this.GetComponent<Item>().Consume();
	}
}
