using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SpriteSheet))]
public class EditorSpriteSheet : Editor {
	
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		
		SpriteSheet script = (SpriteSheet)target;
		
		if(Application.isPlaying && GUILayout.Button("Refresh Atlas")){
			script.RefreshTiles();
		}
	}
}
