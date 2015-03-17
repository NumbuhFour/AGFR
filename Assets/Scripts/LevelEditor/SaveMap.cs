using UnityEngine;
using System.Collections;
using SimpleJSON;
using System;
using System.IO;
using System.Collections.Generic;

public class SaveMap {

	public static void SaveMapToFile(MapData mapData, string filename){
		JSONNode n = new JSONClass();
		Map map = mapData.map;
		List<EditorItem> tiles = mapData.userTiles;
		
		JSONNode jinfo,jtiles,jmap,jents;
		n["info"] = jinfo = new JSONClass();
		n["tiles"]= jtiles = new JSONArray();
		n["map"] = jmap = new JSONArray();
		n["entities"] = jents = new JSONArray();
		
		jinfo.Add("width", new JSONData((int)map.Dimensions.x));
		jinfo.Add("height", new JSONData((int)map.Dimensions.y));
		jinfo.Add ("camera", new JSONData(map.CamType));
		JSONNode jspawn = jinfo["spawn"] = new JSONArray();
		jspawn.Add (new JSONData(mapData.spawnX));
		jspawn.Add (new JSONData(mapData.spawnY));
		jinfo.Add ("lightsOut", new JSONData(mapData.lightsOut));
		jinfo.Add ("haunter", new JSONData(mapData.haunter));
		
		
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
	
	public static bool IsSaveableParameter(object data){
		return (data is string || data is int || data is float || data is double || data is bool);
	}
}