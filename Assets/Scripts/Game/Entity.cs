using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {
	
	public Map map;
	public Vector2 loc;
	
	// Use this for initialization
	public virtual void Start () {
		this.loc = new Vector2(this.transform.localPosition.x/18,this.transform.localPosition.y/18);
	}
	
	// Update is called once per frame
	public virtual void Update () {
	}
	
	public void Move(Vector2 dir){
		if(CanMove(dir)) {
			map.NotifyMove(this, loc+dir, loc);
			loc += dir;
			this.transform.localPosition = loc*18;
		}
	}
	
	public bool CanMove(Vector2 dir){
		Vector2 pos = this.transform.localPosition;
		Vector2 newPos = this.transform.localPosition;
		pos *= 1f/18f;
		newPos *= 1f/18f;
		newPos +=  dir;
		
		Tile to = map.GetTileAt((int)newPos.x, (int)newPos.y);
		Tile fro = map.GetTileAt((int)pos.x, (int)pos.y);
		return to.Solidity <= fro.Solidity;
	}
}
