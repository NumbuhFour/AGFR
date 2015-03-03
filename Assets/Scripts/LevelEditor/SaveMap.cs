using UnityEngine;
using System.Collections;
using SimpleJSON;
using System;
using System.IO;
using System.Collections.Generic;

public class SaveMap {

	public static void SaveMapToFile(Map map, List<EditorItem> tiles, string filename){
		JSONNode n = new JSONClass();
		
		
		JSONNode jinfo,jtiles,jmap,jents;
		n["info"] = jinfo = new JSONClass();
		n["tiles"]= jtiles = new JSONArray();
		n["map"] = jmap = new JSONArray();
		n["entities"] = jents = new JSONArray();
		
		jinfo.Add("width", new JSONData((int)map.Dimensions.x));
		jinfo.Add("height", new JSONData((int)map.Dimensions.y));
		JSONNode jspawn = jinfo["spawn"] = new JSONArray();
		jspawn.Add (new JSONData(0));
		jspawn.Add (new JSONData(0));
		jinfo.Add ("lightsOut", new JSONData(0));
		jinfo.Add ("haunter", new JSONData(0));
		
		
		foreach(EditorItem t in tiles){
			t.SaveToJSON(jtiles);
		}
		
		string[,] mapdata = map.MapArray;
		for(int y = 0; y < map.Dimensions.y; y++){
			JSONNode col = new JSONArray();
			for(int x = 0; x < map.Dimensions.x; x++){
				int flipy = (int)map.Dimensions.y - (y+1);
				string tilename = mapdata[x,flipy];
				TileData td = map.GetTileDataAt(x,flipy);
				if(!td.IsEmpty()){
					td.Save(col, tilename);
				}else{
					col.Add(new JSONData(tilename));
				}
			}
			jmap.Add (col);
			
		}
		
		File.WriteAllText(Environment.CurrentDirectory + "/Assets/Resources/Maps/Test" + @"\" +filename+".json", n.ToString());
	}
}