using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[AddComponentMenu("Scripts/LevelEditor/Editor UI")]
public class EditorUI : MonoBehaviour {

	public Vector2 slotOffset = new Vector2(-72,109);
	public Vector2 slotSpacing = new Vector2(72,-72);
	public Rect mapRect = new Rect(32,119,480-32,567-119);
	
	public MapRender mapRen;
	public MapLoader loader;
	public GameObject mapContainer;
	public GameObject noFileShade;
	public Text coordLabel;
	public UnityEngine.EventSystems.EventSystem events;
	public ChatManager chat;

	private List<EditorItem> tilePresets = new List<EditorItem>();
	public PremadeContainer entities;
	
	public GameObject slotPrefab;
	public GameObject popupPrefab;
	
	public LayerMask UILayer;
	public EditorInvSlot toolSlot;
	
	public GameObject fileTabs;
	public GameObject fileTabPrefab;
	
	public Texture2D normalCursor;
	public Texture2D eraserCursor;
	public Texture2D inspectCursor;
	//public Canvas itemCountCanvas;
	//public GameObject itemCountPrefab;
	
	//private EditorInvTile currentlyDragging = null;
	//private EditorInvSlot lastSlot = null;
	//private EditorInvSlot lastSlotLocation = null;
	
	//public GameObject tilePrefab;
	
	public RectTransform userScroll;
	public RectTransform tileScroll;
	public RectTransform entScroll;
	
	private Dictionary<int, MapData> maps = new Dictionary<int, MapData>();
	private int currentMap = -1;
	private GameObject currentFileTab;
	private string currentTab = "tile"; //ents, tiles, user
	
	private bool popupActive = false;
	//public Collider mapCol;
	
	private SpriteSheet sheet;
	
	// Use this for initialization
	void Start () {
		sheet = Game.GameObject.GetComponent<SpriteSheet>();
		SetTab("tile");
	}
	
	// Update is called once per frame
	void Update () {
		
		if(currentMap != -1 && !popupActive){
			Vector3 mouse = Input.mousePosition;
			if(this.mapRect.Contains(mouse)) {
				Texture2D icon = normalCursor;
				bool inspect = false;
				if(Input.GetMouseButton(1)) icon = eraserCursor;
				else if(Input.GetKey(KeyCode.LeftShift)) {
					icon = inspectCursor;
					inspect = true;
				}
				Cursor.SetCursor(icon, new Vector2(16f,16f), CursorMode.Auto);
				
				if(Input.GetMouseButton (0) && GetTool() != null){ //Left click place
					Vector2 pos = GetMousePos (mouse);
					if(!inspect) {
						maps[currentMap].map.SetTileAt((int)pos.x, (int)pos.y, GetTool().Name);
					}else {
						this.InspectTile(maps[currentMap].map, (int)pos.x, (int)pos.y);
					}
				}else if(Input.GetMouseButton(1)){	//right click erase
					Vector2 pos = GetMousePos (mouse);
					maps[currentMap].map.SetTileAt((int)pos.x, (int)pos.y, null);
				}else if(Input.GetMouseButton (2) && GetTool() != null){
					Vector2 pos = GetMousePos (mouse);
					int x = (int)pos.x;
					int y = (int)pos.y;
					Map map = maps[currentMap].map;
					string from = map.GetTileAt(x,y).name;
					string to = GetTool ().Name;
					if(from != to)
						RecursiveFill(map, x,y, from, to);
				}
			}else {
				Cursor.SetCursor(null, new Vector2(), CursorMode.Auto);
			}
		}else {
			Cursor.SetCursor(null, new Vector2(), CursorMode.Auto);
		}
		
		if(currentMap != -1){
			Vector3 mousePos = Input.mousePosition;
			if(this.mapRect.Contains(mousePos)) {
				Vector2 pos = GetMousePos (mousePos);
				coordLabel.text = "[x: " + (int)pos.x + "\t\ty:" + (int)pos.y + "]";
			}else {
				coordLabel.text = "";
			}
		}
		
		//Scrolling 
		
		if(currentMap != -1 && !popupActive){
			int horiz = (int)Input.GetAxisRaw("Horizontal");
			int vert = (int)Input.GetAxisRaw("Vertical");
			if(horiz != 0 || vert != 0){
				Vector2 add = new Vector2(horiz,vert);
				maps[currentMap].map.CamLoc = maps[currentMap].map.CamLoc + add;
			}
		}
	}
	
	private void InspectTile(Map map, int x, int y){
		EditorPopup popup = MakePopup();
		
		TileData td = map.GetTileDataAt(x,y);
		
		popup.InitInspector(FindUserEditorItem(map.GetTileAt(x,y)), (Dictionary<string, string> data, bool cancelled) => {
			popupActive = false;
			if(!cancelled){
				foreach(string key in data.Keys){
					td[key] = data[key];
				}
			}
		}, null);
		popupActive = true;
		
		foreach (string key in td.Data.Keys){
			object data = td.Data[key];
			if(SaveMap.IsSaveableParameter(data))
				popup.AddEmptyAttrib(key, td.Data[key].ToString());
		}
		
		popup.AddAddButton();
		popup.AddSubmit();
		popup.End();
	}
	
	private void RecursiveFill(Map map, int x, int y, string from, string to){
		if(x < 0 || y < 0 || x >= map.Dimensions.x || y >= map.Dimensions.y) return;
		map.SetTileAt(x,y,to);
		for(int ix = -1; ix <= 1; ix++){
			for(int iy = -1; iy <= 1; iy++){
				if(ix == 0 || iy == 0) //no corners
					if(map.GetTileAt(x+ix, y+iy).name == from) {
						RecursiveFill(map,x+ix,y+iy,from,to);
					}
			}
		}
	}
	
	private EditorItem FindUserEditorItem(Tile t){
		MapData data = maps[currentMap];
		foreach(EditorItem i in data.userTiles){
			if(i.Tile == t) return i;
		}
		return null;
	}
	
	private Vector2 GetMousePos(Vector2 mouse){
		Vector2 pos = (Vector2)mouse - mapRect.min;
		pos = pos/(sheet.tileResolution+2);
		pos = maps[currentMap].map.ConvertSceneToWorld(pos);
		return pos;
	}
	
	public void SetTool(EditorItem tool){	
		if(popupActive || currentMap == -1) return;
		if(tool != null && this.currentTab == "tile"){ //Clicked on tiles tab, add new user tool
			tool = new EditorItem(tool);
			this.AddUserItem(tool);
		}
		this.toolSlot.Item = tool;
	}
	public EditorItem GetTool(){
		return this.toolSlot.Item;
	}
	
	public void ClearUserItems(){
		
	}
	
	public void AddUserItem(EditorItem item){
		int n = 0;
		MapData data = maps[currentMap];
		for(int i = 0; i < data.userTiles.Count; i++){
			if(data.userTiles[i].Name == item.Name || data.userTiles[i].Name == item.Name+"_"+n) n++;
		}
		if(n != 0){
			item.Name += "_" + n;
		}
		data.userTiles.Add(item);
		this.AddPreset(item, userScroll);
	}
	
	public void AddTilePreset(EditorItem item){
		this.AddPreset(item, tileScroll);
		this.tilePresets.Add(item);
	}

	public void AddEntityPreset(EditorItem item){
		this.AddPreset(item, entScroll);	
	}
	
	//Deleted children count is for when preset added immediately after clearing
	//Transforms don't update childCount until next frame I think
	private void AddPreset(EditorItem item, RectTransform container, int deletedChildrenCount=0){
		int index = container.childCount - deletedChildrenCount;
		int y = index%4;
		int x = (int)(index/4);
		GameObject inst = (GameObject)Instantiate(this.slotPrefab);
		inst.transform.SetParent(container);
		inst.transform.localPosition = new Vector3(x*slotSpacing.x + slotOffset.x, y*slotSpacing.y + slotOffset.y,-10);
		inst.GetComponent<EditorInvSlot>().Init(this,item);
		
		Vector2 size = container.sizeDelta;
		float width = (x+1)*slotSpacing.x;
		size.x = Mathf.Max(width,size.x);
		container.sizeDelta = size;
		events.UpdateModules();
		
		UpdateMapPresets();
	}
	
	private void UpdateMapPresets(){
		if(currentMap == -1) return;
		MapData cur = maps[currentMap];
		Map map = cur.map;
		/*foreach(EditorItem e in this.tilePresets){
			cur.SetTile(e.Name, e.Tile);
		}*/
		
		foreach(EditorItem e in cur.userTiles){
			map.SetTile(e.Name, e.Tile);
		}
	}
	
	
	public void SetTab(string tab){
		if(popupActive) return;
		switch(tab){
		case "user":
			this.userScroll.gameObject.SetActive(true);
			this.userScroll.parent.gameObject.GetComponent<Image>().enabled = true;
			this.tileScroll.parent.gameObject.GetComponent<Image>().enabled = false;
			this.entScroll.parent.gameObject.GetComponent<Image>().enabled = false;
			this.userScroll.parent.gameObject.GetComponent<ScrollRect>().enabled = true;
			this.tileScroll.parent.gameObject.GetComponent<ScrollRect>().enabled = false;
			this.entScroll.parent.gameObject.GetComponent<ScrollRect>().enabled = false;
			
			this.tileScroll.gameObject.SetActive(false);
			this.entScroll.gameObject.SetActive(false);
			break;
		case "tile":
			this.tileScroll.gameObject.SetActive(true);
			this.tileScroll.parent.gameObject.GetComponent<Image>().enabled = true;
			this.userScroll.parent.gameObject.GetComponent<Image>().enabled = false;
			this.entScroll.parent.gameObject.GetComponent<Image>().enabled = false;
			this.tileScroll.parent.gameObject.GetComponent<ScrollRect>().enabled = true;
			this.userScroll.parent.gameObject.GetComponent<ScrollRect>().enabled = false;
			this.entScroll.parent.gameObject.GetComponent<ScrollRect>().enabled = false;
			
			this.userScroll.gameObject.SetActive(false);
			this.entScroll.gameObject.SetActive(false);
			break;
		case "entity":
			this.entScroll.gameObject.SetActive(true);
			this.entScroll.parent.gameObject.GetComponent<Image>().enabled = true;
			this.tileScroll.parent.gameObject.GetComponent<Image>().enabled = false;
			this.userScroll.parent.gameObject.GetComponent<Image>().enabled = false;
			this.entScroll.parent.gameObject.GetComponent<ScrollRect>().enabled = true;
			this.tileScroll.parent.gameObject.GetComponent<ScrollRect>().enabled = false;
			this.userScroll.parent.gameObject.GetComponent<ScrollRect>().enabled = false;
			
			this.tileScroll.gameObject.SetActive(false);
			this.userScroll.gameObject.SetActive(false);
			break;
		}
		
		this.currentTab = tab;
	}
	
	public void AddFile(){
		EditorPopup popup = MakePopup();
		
		popup.InitEmpty( (Dictionary<string,string> data, bool cancelled) => {
			popupActive = false;
			if(cancelled) return;
			
			int width = int.Parse(data["width"]);
			int height = int.Parse(data["height"]);
			int mapInd = maps.Count;
			maps[mapInd] = new MapData();
			maps[mapInd].map = mapContainer.AddComponent<Map>();
			maps[mapInd].map.Init(new Vector2(width,height));
			
			Debug.Log("NEW FILE " + width + "x" + height);
			
			GameObject tab = AddFileTab(mapInd);
			SwitchToMap(mapInd, tab);
		});
		popupActive = true;
		
		popup.AddVar("width", "25");
		popup.AddVar("height", "25");
		popup.AddSubmit();
		popup.End();
	}
	
	public void SwitchToMap(int mapInd, GameObject tab) {
		Debug.Log("SWITCHING TO MAP " + mapInd);
		this.noFileShade.SetActive(mapInd == -1);
		this.SetTool(null);
		
		//Clear user tiles tab
		int count = this.userScroll.childCount;
		for(int iter = 0; iter < count; iter++){
			Destroy(this.userScroll.GetChild(iter).gameObject);
			Destroy(this.userScroll.GetChild(iter));
		}
		
		if(mapInd != -1 && maps[mapInd] != null){
			//Set user tiles tab icons to mapData.userTiles
			List<EditorItem> tiles = maps[mapInd].userTiles;
			foreach(EditorItem item in tiles){
				AddPreset(item,this.userScroll, count);
			}
		}
		
		
		this.currentFileTab = tab;
		if(tab){
			UnityEngine.UI.Button butt = tab.GetComponent<UnityEngine.UI.Button>();
			butt.interactable = false; //TODO fix file tab selection glow on creation
		}
		if(currentMap != -1 && maps[currentMap] != null){
			maps[currentMap].map.enabled = false;
		}
		this.currentMap = mapInd;
		if(mapInd != -1) {
			Map m = maps[currentMap].map;
			mapContainer.BroadcastMessage("SetMap", m);
			m.enabled = true;
			m.MarkDirty();
			UpdateMapPresets();
		}
		else {
			mapContainer.BroadcastMessage("SetMap", null);
			this.mapRen.Clear();
		}
		UnsetFileTabs();
	}
	
	private GameObject AddFileTab(int ind){
		int i = 0;
		Vector3 newPos = Vector3.zero;
		for(i = 0; i < this.fileTabs.transform.childCount; i++){
			Transform t = this.fileTabs.transform.GetChild (i);
			Vector3 pos = t.position;
			newPos = pos;
			pos.x += ((RectTransform)t).sizeDelta.x;
			t.position = pos;
		}
		GameObject add = (GameObject)GameObject.Instantiate (fileTabPrefab);
		add.transform.SetParent(fileTabs.transform);
		add.transform.position = newPos;
		add.GetComponent<FileTabButton>().Init(this,ind);
		
		return add;
	}
	
	private void UpdateFileTabs(){
		float nextX = 0;
		for(int i = this.fileTabs.transform.childCount-1; i >= 0; i--){
			Transform t = this.fileTabs.transform.GetChild (i);
			if(!t.gameObject.activeSelf) continue;
			
			t.transform.localPosition = new Vector3(nextX, 0, 0);
			nextX += ((RectTransform)t).sizeDelta.x;
		}
	}
	
	private void UnsetFileTabs(){ //For the glowy effect
		for(int i = 0; i < this.fileTabs.transform.childCount; i++){
			Transform t = this.fileTabs.transform.GetChild (i);
			t.gameObject.GetComponent<UnityEngine.UI.Button>().interactable = true;
		}
	}
	
	public void EditToolProperties(){
		EditorPopup popup = MakePopup();
		EditorItem tool = GetTool ();
		EditorItem backup = new EditorItem(tool);
		EditorPopup.OnChange changer = (Dictionary<string,string> data) => {
			try{
				tool.SpriteID = int.Parse (data["sprite"]);
				tool.Type = data["type"];
				tool.Solidity = int.Parse(data["solidity"]);
				Color c = tool.MainColor;
				c.r = float.Parse(data["color [r]"])/255f;
				c.g = float.Parse(data["color [g]"])/255f;
				c.b = float.Parse(data["color [b]"])/255f;
				tool.MainColor = c;
			}catch(System.Exception ex){} //Bad number
			maps[currentMap].map.MarkDirty();
		};
		EditorPopup.OnClose closer = (Dictionary<string,string> data, bool cancelled) => {
			popupActive = false;
			if(cancelled){
				tool.Type = backup.Type;
				tool.SpriteID = backup.SpriteID;
				tool.Solidity = backup.Solidity;
				tool.MainColor = backup.MainColor;
			}else{
				changer(data);
			}
		};
		popup.InitStyle(tool, closer, changer);
		popupActive = true;
		popup.AddVar("type","" + tool.Type);
		popup.AddVar("sprite","" + tool.SpriteID);
		popup.AddVar("solidity","" + tool.Solidity);
		popup.AddVar("color [r]", "" + (int)(tool.MainColor.r * 255));
		popup.AddVar("color [g]", "" + (int)(tool.MainColor.g * 255));
		popup.AddVar("color [b]", "" + (int)(tool.MainColor.b * 255));
		popup.AddSubmit();
		popup.End();
	}
	
	private EditorPopup MakePopup(){
		GameObject popupGO = (GameObject)Instantiate(popupPrefab);
		popupGO.transform.SetParent(this.transform);
		EditorPopup popup = popupGO.GetComponent<EditorPopup>();
		popup.transform.localPosition = Vector3.zero;
		return popup;
	}
	
	public void Save(){
		if(currentMap == -1) {
			chat.PushText("", "No file open, can't save!");
		}
		
		MapData mapdata = maps[currentMap];
		Map map = mapdata.map;
		EditorPopup popup = MakePopup();
		popup.InitEmpty( (Dictionary<string,string> data, bool cancelled) => {
			//On close
			popupActive = false;
			if(!cancelled){
				map.mapName = data["filename"];
				SaveMap.SaveMapToFile(mapdata, map.mapName);
				chat.PushText("","Map saved!");
			}
		});
		popupActive = true;
		
		popup.AddVar("filename", map.mapName);
		popup.AddSubmit();
		popup.End();
	}
	
	public void PromptOpenFile(){
		EditorPopup popup = MakePopup();
		
		popup.InitEmpty((Dictionary<string,string> data, bool cancelled) => {
			//On close
			popupActive = false;
			if(!cancelled){
				string filename = data["file name"];
				
				TextAsset file = (TextAsset) Resources.LoadAssetAtPath<TextAsset>("Assets/Resources/Maps/" + filename + ".json");
				if(file != null){
					
					int mapInd = maps.Count;
					maps[mapInd] = new MapData();
					Map map = maps[mapInd].map = mapContainer.AddComponent<Map>();
					
					GameObject tab = AddFileTab(mapInd);
					
					loader.Load(maps[mapInd], file);
					
					SwitchToMap(mapInd,tab);
					
					Debug.Log("Loaded map: " + filename + " Dimensions: " + map.Dimensions);
				}else{
					this.chat.PushText("","File not found!");
				}
			}
		});
		this.popupActive = true;
		
		popup.AddVar("file name", "");
		popup.AddSubmit();
		popup.End();
	}
	
	public void PromptCloseFile(){
		
		if(currentMap == -1) {
			chat.PushText("", "No file open!");
			return;
		}
		EditorPopup popup = MakePopup();
		
		popup.InitEmpty((Dictionary<string,string> data, bool cancelled) => {
			//On Close
			popupActive = false;
			if(!cancelled){
				//Delete map file
				Debug.Log ("Closed!");
				
				this.currentFileTab.SetActive(false); 
				Destroy (this.currentFileTab);
				Destroy (maps[this.currentMap].map);
				maps[this.currentMap] = null;
				
				UpdateFileTabs();
				this.currentMap = -1;
				this.currentFileTab = null;
				this.SwitchToMap(-1,null);
			}
		});
		popupActive = true;
		
		popup.AddLabel("Are you sure you want to close this?");
		popup.AddSubmit();
		popup.End();
	}
	
	public void PromptMapProperties(){if(currentMap == -1) {
			chat.PushText("", "No file open!");
			return;
		}
		EditorPopup popup = MakePopup();
		
		MapData map = maps[currentMap];
		popup.InitEmpty((Dictionary<string,string> data, bool cancelled) => {
			//On Close
			popupActive = false;
			if(!cancelled){
				map.spawnX = int.Parse(data["spawn x"]);
				map.spawnY = int.Parse(data["spawn y"]);
				this.chat.PushText("","Success!");
			}
		});
		popupActive = true;
		
		popup.AddVar("spawn x", map.spawnX + "");
		popup.AddVar("spawn y", map.spawnY + "");
		popup.AddSubmit();
		popup.End();
	}
}