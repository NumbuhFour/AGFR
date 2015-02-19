using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SetRenderLayer : MonoBehaviour {

	public string renderLayerName;
	public int renderLayer = 0;

	// Use this for initialization
	void Start () {
		this.renderer.sortingLayerName = renderLayerName;
		this.renderer.sortingOrder = this.renderLayer;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
