using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Test/HideOnPlay")]
public class HideOnPlay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
