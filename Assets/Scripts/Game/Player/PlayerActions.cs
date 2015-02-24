using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(FaceDirection))]
[AddComponentMenu("Scripts/Game/Player/Player Use")]
public class PlayerActions : MonoBehaviour {
	private Entity ent;
	private Map map;
	private FaceDirection fdir;
	private bool wasPressed = false;
	
	void Start(){
		ent = gameObject.GetComponent<Entity>();
		map = ent.map;
		fdir = gameObject.GetComponent<FaceDirection>();
	}
	
	// Update is called once per frame
	void Update () {
		bool pressed = Input.GetAxisRaw("Use")>0;
		
		if(!wasPressed && pressed){
			map.UseTile(ent.loc + Direction.ConvertToVector(fdir.direction), ent);
		}
		
		wasPressed = pressed;
	}
}
