﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileData {
	
	private Dictionary<string, object> data = new Dictionary<string, object>();
	private Vector2 pos;
	
	public TileData(float x, float y){
		this.x = (int)x;
		this.y = (int)y;
	}
	
	public TileData(Vector2 pos){
		this.x = (int)pos.x;
		this.y = (int)pos.y;
	}
	
	public object this[string key]{
		get { 
			if(key == "x") return (int)pos.x;
			if(key == "y") return (int)pos.y;
			if(data.ContainsKey(key))
				return data[key]; 
			else return null;
		}
		set { 
			if(key == "x") pos.x = (int)value;
			else if(key == "y") pos.y = (int)value;
			else data[key] = value; 
		}
	}
	
	public int x{
		get { return (int)pos.x; }
		set { pos.x = (int)value; }
	}
	
	public int y{
		get { return (int)pos.y; }
		set { pos.y = (int)value; }
	}
}
