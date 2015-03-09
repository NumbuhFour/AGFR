using UnityEngine;
using System.Collections;

public class SpriteSelector : MonoBehaviour {
	
	public delegate void OnSubmit(int spriteID);

	public GameObject selector;
	public GameObject atlas;
	private Vector2 atlasDim;

	private int spriteID;
	private OnSubmit callback;
	
	private bool dragging;
	private Vector2 dragOffset;
	
	private Vector2 selectedTile;

	public void Init(int spriteID, OnSubmit callback){
		this.spriteID = spriteID;
		this.callback = callback;
		atlasDim = ((RectTransform)atlas.transform).sizeDelta;
		
		Vector2 atlasTiles = atlasDim/16;
		selectedTile = new Vector2(spriteID%atlasTiles.x, atlasTiles.y - Mathf.Ceil(spriteID/atlasTiles.x));
		selector.transform.localPosition = selectedTile*16;
	}
	
	// Update is called once per frame
	void Update () {
		//Track mouse
		Vector2 mouse = Input.mousePosition;
		if(!dragging && Input.GetMouseButton(2)){ //Middle mouse press
			dragging = true;
			dragOffset = (Vector2)atlas.transform.position - mouse;
		}else if(dragging && !Input.GetMouseButton(2)){
			dragging = false;
		}else if(dragging){
			atlas.transform.position = (Vector3)mouse + (Vector3)dragOffset;
		}
		
		if(Input.GetMouseButtonUp(0)){ //Click
			Vector3 clickPos = atlas.transform.InverseTransformPoint((Vector3)mouse);
			
			clickPos.x -= clickPos.x%16;
			clickPos.y -= clickPos.y%16;
			
			if(clickPos.x >= 0 && clickPos.y >= 0 && clickPos.x < atlasDim.x && clickPos.y < atlasDim.y){
				selectedTile = clickPos/16;
				clickPos = atlas.transform.TransformPoint(clickPos);
				selector.transform.position = clickPos;
			}
		}
	}
	
	//Called by cancel button
	public void OnCancelButt(){
		Close(true);
	}
	
	//Called by submit button
	public void OnSubmitButt(){
		Close(false);
	}
	
	private int CalcSpriteID(){
		
		Vector2 spriteDim = atlasDim/16;
		Vector2 selection = new Vector2(this.selectedTile.x, spriteDim.y - this.selectedTile.y - 1);
		return (int)(selection.y * spriteDim.x + selection.x);
	}
	
	private void Close(bool cancelled){
		if(!cancelled){
			spriteID = CalcSpriteID();
		}
		
		if(callback != null) this.callback(spriteID);
		Destroy (this.gameObject);
	}
}
