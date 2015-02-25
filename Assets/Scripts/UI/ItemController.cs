using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/UI/Item Controller")]
public class ItemController : MonoBehaviour {

	public InventoryUI inv;
	private GameObject player;
	private FaceDirection dir;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		dir = player.GetComponent<FaceDirection>();
	}
	
	private int lastHoriz = 0;
	private int lastVert = 0;
	// Update is called once per frame
	void Update () {
		if(Game.Paused) return;
		if(inv.GetWeapon() == null) return;
		
		int horiz = (int)Input.GetAxisRaw("FireHorizontal");
		int vert = (int)Input.GetAxisRaw("FireVertical");
		
		if(horiz != lastHoriz && horiz != 0 && ((int)Input.GetAxisRaw("Horizontal") + horiz) != 0){
			inv.GetWeapon().SendMessage("OnSwing", horiz>0 ? "east":"west");
			dir.SetDirection(horiz>0 ? "east":"west");
		}
		if(vert != lastVert && vert != 0 && ((int)Input.GetAxisRaw("Vertical") + vert) != 0){
			inv.GetWeapon().SendMessage("OnSwing", vert>0 ? "north":"south");
			dir.SetDirection(vert>0 ? "north":"south");
		}
		
		lastHoriz = horiz;
		lastVert = vert;
	}
}
