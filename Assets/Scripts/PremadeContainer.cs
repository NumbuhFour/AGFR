using UnityEngine;
using System.Collections;

public class PremadeContainer : ScriptableObject {

	public GameObject[] prefabs;
	
	public GameObject this[string key] {
		get {
			foreach(GameObject go in prefabs){
				if(((INamed)go.GetComponent(typeof(INamed))).Name() == key) return go;
			}
			return null;
		}
	}
}
