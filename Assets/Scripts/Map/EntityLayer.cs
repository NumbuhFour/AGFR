using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Map/Entity Layer")]
public class EntityLayer : MonoBehaviour {
	public Map map;
	private Entity[,] entCols;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void Init(Vector2 dimensions){
		entCols = new Entity[(int)dimensions.x,(int)dimensions.y];
	}
	
	public void NotifyMove(Entity e, Vector2 to, Vector2 from){
		Tile toTile = map.GetTileAt((int)to.x, (int)to.y);
		TileData toData = map.GetTileDataAt((int)to.x,(int)to.y);
		entCols[(int)to.x, (int)to.y] = e;
		
		if(from.x != -1){
			Tile fromTile = map.GetTileAt((int)from.x, (int)from.y);
			TileData fromData = map.GetTileDataAt((int)from.x,(int)from.y);
			fromTile.OnEntityExit(e,fromData);
			entCols[(int)from.x, (int)from.y] = null;
		}
		
		toTile.OnEntityEnter(e,toData);
	}
	
	public bool Occupied(Vector2 loc){
		int xo = (int)loc.x;
		int yo = (int)loc.y;
		if(xo < 0 || xo >= Map.MAPDIM.x) return true;
		if(yo < 0 || yo >= Map.MAPDIM.y) return true;
		return (entCols[(int)loc.x, (int)loc.y] != null);
	}
	public Entity GetEntityAt(Vector2 loc){
		return entCols[(int)loc.x, (int)loc.y];
	}
	
	public GameObject SpawnEntity(GameObject prefab, Vector2 pos){
		GameObject spawn = (GameObject)Instantiate(prefab);
		spawn.transform.parent = this.transform;
		Entity ent = spawn.GetComponent<Entity>();
		ent.map = this.map;
		ent.entlayer = this;
		ent.SetPos(pos);
		return spawn;
	}
}
