using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {
	
	public Map map;
	
	// Use this for initialization
	public virtual void Start () {
	}
	
	// Update is called once per frame
	public virtual void Update () {
	}
	
	public void Move(Vector2 dir){
		if(CanMove(dir)) this.transform.position += new Vector3(dir.x*18,dir.y*18,0);
	}
	
	public bool CanMove(Vector2 dir){
		Vector2 pos = this.transform.position;
		Vector2 newPos = this.transform.position;
		pos *= 1f/18f;
		newPos *= 1f/18f;
		newPos +=  dir;
		
		Debug.Log("ASDASDA S" + pos + " " + newPos);
		Tile to = map.GetTileAt((int)newPos.x, (int)newPos.y);
		Tile fro = map.GetTileAt((int)pos.x, (int)pos.y);
		Debug.Log("Fuck you " + to.Name + " " + fro.Name + " [" + to.Solidity + "," + fro.Solidity + "]");
		return to.Solidity <= fro.Solidity;
	}
}
