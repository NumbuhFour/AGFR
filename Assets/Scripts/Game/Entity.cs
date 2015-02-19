using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour, INamed {
	
	public Map map;
	public EntityLayer entlayer;
	public Vector2 loc;
	public string name;
	
	private Vector2 lastLoc = new Vector2(-1,-1);
	public Vector2 LastLoc { get{ return lastLoc; } }
	
	public string Name() { return name; }
	
	// Use this for initialization
	public virtual void Start () {
		this.loc = new Vector2(this.transform.localPosition.x/18,this.transform.localPosition.y/18);
	}
	
	// Update is called once per frame
	public virtual void Update () {
	}
	
	public void Move(Vector2 dir){
		if(dir == Vector2.zero) return;
		if(CanMove(dir)) {
			entlayer.NotifyMove(this, loc+dir, loc);
			lastLoc = loc;
			loc += dir;
			this.transform.localPosition = loc*18;
		}
	}
	
	public void SetPos(Vector2 pos, bool resetLastLoc=false){
		if(resetLastLoc)lastLoc = pos;
		else lastLoc = pos;
		
		entlayer.NotifyMove(this, pos, new Vector2(-1,-1));
		loc = pos;
		this.transform.localPosition = loc*18;
	}
	
	public bool CanMove(Vector2 dir){
		Vector2 pos = loc;
		Vector2 newPos = loc;
		newPos += dir;
		if(entlayer.Occupied(newPos)) return false;
		
		Tile to = map.GetTileAt((int)newPos.x, (int)newPos.y);
		Tile fro = map.GetTileAt((int)pos.x, (int)pos.y);
		return to.Solidity <= fro.Solidity;
	}
}
