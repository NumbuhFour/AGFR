﻿using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Face Direction")]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Entity))]
public class FaceDirection : MonoBehaviour {

	[Header("East,North,West,South,Idle")]
	public Sprite[] directions;
	
	// Use this for initialization
	void Start () {
	}
	
	public void SetDirection(string direction){
		int dir = ConvertDir(direction);
		((SpriteRenderer)renderer).sprite = directions[dir];
	}
	private int ConvertDir(string dir){
		switch(dir.ToLower()){
		case"idle": return 4;
		case "east": return 0;
		case "north": return 1;
		case "west": return 2;
		case "south": return 3;
		}return 0;
	}
	
}
