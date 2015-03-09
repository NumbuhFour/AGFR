using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SetImageToTileAtlas))]
public class EditorSetImageToTileAtlas : Editor {
	
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		
		SetImageToTileAtlas script = (SetImageToTileAtlas)target;
		
		if(GUILayout.Button("Refresh Atlas")){
			script.RefreshAtlas();
		}
	}
}
