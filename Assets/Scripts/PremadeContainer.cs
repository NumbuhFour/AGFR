using UnityEngine;
using System.Collections;

public class PremadeContainer : ScriptableObject {

	public GameObject[] prefabs;
	
	public GameObject this[string key] {
		get {
			foreach(GameObject go in prefabs){
				if(go.GetComponent<Entity>().name == key) return go;
			}
			return null;
		}
	}
}
