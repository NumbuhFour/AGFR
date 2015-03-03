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
	public UnityEngine.EventSystems.EventSystem events;
	public ChatManager chat;

	private List<EditorItem> tilePresets = new List<EditorItem>();
	private List<EditorItem> userTiles = new List<EditorItem>();
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
	
	private Dictionary<int, Map> maps = new Dictionary<int, Map>();
	private int currentMap = -1;
	
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
						maps[currentMap].SetTileAt((int)pos.x, (int)pos.y, GetTool().Name);
					}else {
						this.InspectTile(maps[currentMap], (int)pos.x, (int)pos.y);
					}
				}else if(Input.GetMouseButton(1)){	//right click erase
					Vector2 pos = GetMousePos (mouse);
					maps[currentMap].SetTileAt((int)pos.x, (int)pos.y, null);
				}else if(Input.GetMouseButton (2) && GetTool() != null){
					Vector2 pos = GetMousePos (mouse);
					int x = (int)pos.x;
					int y = (int)pos.y;
					Map map = maps[currentMap];
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
		
		//Scrolling 
		
		if(currentMap != -1 && !popupActive){
			int horiz = (int)Input.GetAxisRaw("Horizontal");
			int vert = (int)Input.GetAxisRaw("Vertical");
			if(horiz != 0 || vert != 0){
				Vector2 add = new Vector2(horiz,vert);
				maps[currentMap].CamLoc = maps[currentMap].CamLoc + add;
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
			popup.AddEmptyAttrib(key, (string)td.Data[key]);
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
		foreach(EditorItem i in userTiles){
			if(i.Tile == t) return i;
		}
		return null;
	}
	
	private Vector2 GetMousePos(Vector2 mouse){
		Vector2 pos = (Vector2)mouse - mapRect.min;
		pos = pos/(sheet.tileResolution+2);
		pos = maps[currentMap].ConvertSceneToWorld(pos);
		return pos;
	}
	
	public void SetTool(EditorItem tool){	
		if(popupActive) return;
		if(this.tilePresets.Contains(tool)){
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
		for(int i = 0; i < userTiles.Count; i++){
			if(userTiles[i].Name == item.Name || userTiles[i].Name == item.Name+"_"+n) n++;
		}
		if(n != 0){
			item.Name += "_" + n;
		}
		Debug.Log("NEW USER TILE " + item.Name);
		this.userTiles.Add(item);
		this.AddPreset(item, userScroll);
	}
	
	public void AddTilePreset(EditorItem item){
		this.AddPreset(item, tileScroll);
		this.tilePresets.Add(item);
	}

	public void AddEntityPreset(EditorItem item){
		this.AddPreset(item, entScroll);	
	}
	
	private void AddPreset(EditorItem item, RectTransform container){
		int index = container.childCount;
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
		Map cur = maps[currentMap];
		/*foreach(EditorItem e in this.tilePresets){
			cur.SetTile(e.Name, e.Tile);
		}*/
		
		foreach(EditorItem e in this.userTiles){
			cur.SetTile(e.Name, e.Tile);
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
	}
	
	public void AddFile(){
		EditorPopup popup = MakePopup();
		
		popup.InitEmpty( (Dictionary<string,string> data, bool cancelled) => {
			popupActive = false;
			if(cancelled) return;
			
			int width = int.Parse(data["width"]);
			int height = int.Parse(data["height"]);
			int mapInd = maps.Count;
			maps[mapInd] = this.gameObject.AddComponent<Map>();
			maps[mapInd].Init(new Vector2(width,height));
			
			Debug.Log("NEW FILE " + width + "x" + height);
			
			SwitchToMap(mapInd);
			AddFileTab(mapInd);
		});
		popupActive = true;
		
		popup.AddVar("width", "25");
		popup.AddVar("height", "25");
		popup.AddSubmit();
		popup.End();
	}
	
	public void SwitchToMap(int i) {
		Debug.Log("SWITCHING TO MAP " + i);
		if(currentMap != -1 && maps[currentMap]){
			maps[currentMap].enabled = false;
		}
		this.currentMap = i;
		this.mapRen.map = maps[currentMap];
		this.mapRen.map.enabled = true;
		this.mapRen.map.MarkDirty();
		UpdateMapPresets();
		UnsetFileTabs();
	}
	
	private void AddFileTab(int ind){
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
			maps[currentMap].MarkDirty();
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
		
		Map map = maps[currentMap];
		EditorPopup popup = MakePopup();
		popup.InitEmpty( (Dictionary<string,string> data, bool cancelled) => {
			popupActive = false;
			if(!cancelled){
				map.mapName = data["filename"];
				SaveMap.SaveMapToFile(map, this.userTiles, map.mapName);
				chat.PushText("","Map saved!");
			}
		});
		popupActive = true;
		
		popup.AddVar("filename", map.mapName);
		popup.AddSubmit();
		popup.End();
	}
}