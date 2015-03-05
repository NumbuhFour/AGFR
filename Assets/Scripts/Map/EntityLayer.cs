using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Map/Entity Layer")]
public class EntityLayer : MonoBehaviour {
	public Map map;
	private Entity[,] entCols;
	private bool initialized = false;
	private Vector2 dimensions;
	
	public void Init(Vector2 dimensions){
		this.dimensions = dimensions;
		entCols = new Entity[(int)dimensions.x,(int)dimensions.y];
		initialized = true;
	}
	
	public void NotifyMove(Entity e, Vector2 to, Vector2 from){
		Tile toTile = map.GetTileAt((int)to.x, (int)to.y);
		TileData toData = map.GetTileDataAt((int)to.x,(int)to.y);
		entCols[(int)to.x, (int)to.y] = e;
		
		if(from.x != -1){
			Tile fromTile = map.GetTileAt((int)from.x, (int)from.y);
			TileData fromData = map.GetTileDataAt((int)from.x,(int)from.y);
			fromTile.OnEntityExit(e,fromData);
			if(entCols[(int)from.x, (int)from.y] == e)
				entCols[(int)from.x, (int)from.y] = null;
		}
		
		toTile.OnEntityEnter(e,toData);
	}
	
	public bool Occupied(Vector2 loc){
		if(loc.x >= dimensions.x || loc.x < 0 || 
		   loc.y >= dimensions.y || loc.y < 0) return true;
		return (entCols[(int)loc.x, (int)loc.y] != null);
	}
	public Entity GetEntityAt(Vector2 loc){
		if(loc.x >= dimensions.x || loc.x < 0 || 
		   loc.y >= dimensions.y || loc.y < 0) return null;
		return entCols[(int)loc.x, (int)loc.y];
	}
	
	public GameObject SpawnEntity(GameObject prefab, Vector2 pos){
		if(pos.x >= dimensions.x || pos.x < 0 || 
		   pos.y >= dimensions.y || pos.y < 0) return null;
		GameObject spawn = (GameObject)Instantiate(prefab);
		spawn.transform.parent = this.transform;
		Entity ent = spawn.GetComponent<Entity>();
		ent.map = this.map;
		ent.entlayer = this;
		ent.SetPos(pos);
		entCols[(int)ent.loc.x, (int)ent.loc.y] = ent;
		return spawn;
	}
	
	public bool UseEntity(Vector2 loc, Entity e){
		Entity ent = GetEntityAt(loc);
		if(ent){
			ent.gameObject.SendMessage("OnUse", e, SendMessageOptions.DontRequireReceiver);
			return true;
		}
		return false;
	}
	public void RemoveEntity(Entity e){
		if(map.IsLocValid(e.loc) && entCols[(int)e.loc.x, (int)e.loc.y] == e)
			entCols[(int)e.loc.x, (int)e.loc.y] = null;
	}
	
	//public void OnLevelReset() { this.Clear(); }
	
	public void Clear(){
		if(initialized)
			for(int x = 0; x < this.dimensions.x; x++)
				for(int y = 0; y < this.dimensions.y; y++){
					Entity e = entCols[x,y];
					if(e) Destroy (e.gameObject);
				}
		initialized = false;
	}
	
	public Vector2 ConvertEntityPosToScenePos(Vector2 loc){
		Vector2 conv = map.ConvertWorldToScene(loc);
		Debug.Log("ENT POS " + conv + " " + loc);
		conv *= map.sheet.tileResolution+2;
		return conv;
	}
}
