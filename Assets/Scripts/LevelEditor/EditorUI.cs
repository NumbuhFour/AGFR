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

	private List<EditorItem> tilePresets = new List<EditorItem>();
	private List<EditorItem> userTiles = new List<EditorItem>();
	public PremadeContainer entities;
	
	public GameObject slotPrefab;
	public GameObject popupPrefab;
	
	public LayerMask UILayer;
	public EditorInvSlot toolSlot;
	
	public GameObject fileTabs;
	public GameObject fileTabPrefab;
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
	//public Collider mapCol;
	
	private SpriteSheet sheet;
	
	// Use this for initialization
	void Start () {
		sheet = Game.GameObject.GetComponent<SpriteSheet>();
		SetTab("tile");
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr2","tile",33,Color.green,Color.white,1));
		/*AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));
		AddTilePreset(new EditorItem("rawr","tile",32,Color.red,Color.white,1));*/
	}
	
	// Update is called once per frame
	void Update () {
		
		if(currentMap != -1 && Input.GetMouseButton (0) && this.toolSlot.Item != null){
			Vector3 mouse = Input.mousePosition;
			if(this.mapRect.Contains(mouse)) {
				Vector2 pos = (Vector2)mouse - mapRect.min;
				pos = pos/(sheet.tileResolution+2);
				pos = maps[currentMap].ConvertSceneToWorld(pos);
				Debug.Log("POS " + pos);
				maps[currentMap].SetTileAt((int)pos.x, (int)pos.y, toolSlot.Item.Name);
			}
		}
	}
	
	public void SetTool(EditorItem tool){
		this.toolSlot.Item = tool;
	}
	
	public void ClearUserItems(){
		
	}
	
	public void AddUserItem(EditorItem item){
		this.AddPreset(item, userScroll);
		this.userTiles.Add(item);
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
		foreach(EditorItem e in this.tilePresets){
			cur.SetTile(e.Name, e.Tile);
		}
		
		foreach(EditorItem e in this.userTiles){
			cur.SetTile(e.Name, e.Tile);
		}
	}
	
	
	public void SetTab(string tab){
		switch(tab){
		case "user":
			this.userScroll.gameObject.SetActive(true);
			this.userScroll.parent.gameObject.GetComponent<Image>().enabled = true;
			this.tileScroll.parent.gameObject.GetComponent<Image>().enabled = false;
			this.entScroll.parent.gameObject.GetComponent<Image>().enabled = false;
			
			this.tileScroll.gameObject.SetActive(false);
			this.entScroll.gameObject.SetActive(false);
			break;
		case "tile":
			this.tileScroll.gameObject.SetActive(true);
			this.tileScroll.parent.gameObject.GetComponent<Image>().enabled = true;
			this.userScroll.parent.gameObject.GetComponent<Image>().enabled = false;
			this.entScroll.parent.gameObject.GetComponent<Image>().enabled = false;
			
			this.userScroll.gameObject.SetActive(false);
			this.entScroll.gameObject.SetActive(false);
			break;
		case "entity":
			this.entScroll.gameObject.SetActive(true);
			this.entScroll.parent.gameObject.GetComponent<Image>().enabled = true;
			this.tileScroll.parent.gameObject.GetComponent<Image>().enabled = false;
			this.userScroll.parent.gameObject.GetComponent<Image>().enabled = false;
			
			this.tileScroll.gameObject.SetActive(false);
			this.userScroll.gameObject.SetActive(false);
			break;
		}
	}
	
	public void AddFile(){
		GameObject popupGO = (GameObject)Instantiate(popupPrefab);
		popupGO.transform.SetParent(this.transform);
		EditorPopup popup = popupGO.GetComponent<EditorPopup>();
		popup.transform.localPosition = Vector3.zero;
		
		popup.InitEmpty( (Dictionary<string,string> data, bool cancelled) => {
			if(cancelled) return;
			
			AddFileTab();
			int width = int.Parse(data["width"]);
			int height = int.Parse(data["height"]);
			int mapInd = maps.Count;
			maps[mapInd] = this.gameObject.AddComponent<Map>();
			maps[mapInd].Init(new Vector2(width,height));
			
			Debug.Log("NEW FILE " + width + "x" + height);
			
			SwitchToMap(mapInd);
		});
		
		popup.AddVar("width", "25");
		popup.AddVar("height", "25");
		popup.AddSubmit();
		popup.End();
	}
	
	private void SwitchToMap(int i) {
		if(currentMap != -1 && maps[currentMap]){
			maps[currentMap].enabled = false;
		}
		this.currentMap = i;
		this.mapRen.map = maps[currentMap];
		this.mapRen.map.enabled = true;
		this.mapRen.map.MarkDirty();
		UpdateMapPresets();
	}
	
	private void AddFileTab(){
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
		add.GetComponent<FileTabButton>().Init(this,maps.Count);
	}
}