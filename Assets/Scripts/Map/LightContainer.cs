using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Map/Light Container")]
public class LightContainer : MonoBehaviour {

	public Map map;
	public GameObject lightPrefab;
	private Vector3 startingLoc = new Vector3(16,16,192);
	
	//Called via SendMessage from map
	public void CameraMove(){
		Vector3 change = map.CamLoc;
		this.transform.localPosition = -(Vector3)change*(map.sheet.tileResolution+2) + this.startingLoc;
	}
	
	public TileLight AddLight(int x, int y){
		float tileRes = map.sheet.tileResolution+2;
		GameObject add = (GameObject)Instantiate(lightPrefab);
		add.transform.SetParent(this.transform);
		add.transform.localPosition = new Vector3(x*tileRes, y*tileRes, 0);
		return add.GetComponent<TileLight>();
	}
}
