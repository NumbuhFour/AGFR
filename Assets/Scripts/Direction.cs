using UnityEngine;
using System.Collections;

public class Direction
{
	
	public static int ConvertToIndex(string dir){
		switch(dir.ToLower()){
		case"idle": return 4;
		case "east": return 0;
		case "north": return 1;
		case "west": return 2;
		case "south": return 3;
		}return 0;
	}	
	
	public static Vector2 ConvertToVector(string dir){
		return ConvertToVector(ConvertToIndex(dir));
	}	
	
	public static Vector2 ConvertToVector(int dir){
		switch(dir){
		case 0: return new Vector2(1,0);
		case 2: return new Vector2(-1,0);
		case 1: return new Vector2(0,1);
		case 3: return new Vector2(0,-1);
		}
		return new Vector2(0,-1);
	}
}

