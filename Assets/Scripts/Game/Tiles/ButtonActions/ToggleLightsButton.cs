using UnityEngine;
using System.Collections;

public class ToggleLightsButton : IButtonAction
{
	private TileData data;
	private LightNoise lights;
	public ToggleLightsButton(TileData data){
		this.data = data;
		this.lights = GameObject.FindGameObjectWithTag("LightOverlay").GetComponent<LightNoise>();
	}
	
	public void OnEntityEnter(Entity e){
		lights.LightsOut = !lights.LightsOut;
	}
	public void OnEntityExit(Entity e){
	}
	public void Update(){
		
	}
}

