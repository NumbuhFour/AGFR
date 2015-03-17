using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[AddComponentMenu("Scripts/LevelEditor/Popup")]
public class EditorPopup : MonoBehaviour {
	
	public delegate void OnClose(Dictionary<string,string> data, bool cancelled);
	public delegate void OnChange(Dictionary<string,string> data);

	public struct Attribute {
		public string title;
		public string value;
	}

	public GameObject topPrefab;
	public GameObject submitPrefab;
	public GameObject bottomPrefab;
	
	public GameObject styleTopPrefab;
	public GameObject attribTopPrefab;
	
	public GameObject styleVarPrefab;
	public GameObject attribVarPrefab;
	
	public GameObject addAttribPrefab;
	
	public GameObject labelPrefab;
	
	public GameObject selectorPrefab;
	public GameObject spriteSelector;
	
	private List<GameObject> pieces;
	
	public Dictionary<string, string> values; //For preset attributes (x, y, width, etc)
	public List<Attribute> attributes; //For attribute entries with changing name

	private OnClose closer;
	private OnChange changer;

	private bool ready = false;
	private float originY;
	// Use this for initialization
	void Start () {
		originY = 0;//this.transform.localPosition.y;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void InitEmpty(OnClose closer, OnChange changer=null){
		this.closer = closer;
		this.changer = changer;
		pieces = new List<GameObject>();
		GameObject top = AddPiece(topPrefab);
		top.transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { Close (true); } );
	}
	
	public void InitStyle(EditorItem item, OnClose closer=null, OnChange changer=null){
		this.closer = closer;
		this.changer = changer;
		pieces = new List<GameObject>();
		GameObject top = AddPiece(styleTopPrefab);
		top.transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { Close (true); } );
		top.transform.GetChild(1).GetComponent<EditorInvSlot>().Init(null, item);
	}
	
	public void InitInspector(EditorItem item, OnClose closer=null, OnChange changer=null){
		this.closer = closer;
		this.changer = changer;
		pieces = new List<GameObject>();
		GameObject top = AddPiece(attribTopPrefab);
		top.transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { Close (true); } );
		top.transform.GetChild(1).GetComponent<EditorInvSlot>().Init(null, item);
	}
	
	public void End(){
		AddPiece(bottomPrefab);
		this.ready = true;
	}
	
	public void AddAddButton() {
		GameObject add = AddPiece(addAttribPrefab);
	
		//GameObject btn = add.transform.GetChild(0).gameObject;
		
		add.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { 
			InsertEmptyAttrib("title","value");
		} );
	}
	
	public void AddLabel(string message){
		GameObject add = AddPiece (labelPrefab);
		
		add.transform.GetChild(0).GetComponent<Text>().text = message;
	}
	
	public void AddSubmit(){
		GameObject add = AddPiece(submitPrefab);
		
		GameObject submit = add.transform.GetChild(0).gameObject;
		GameObject cancel = add.transform.GetChild(1).gameObject;
		
		submit.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { Close (false); } );
		cancel.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { Close (true); } );
	}
	
	public void AddVar(string title, string defaultValue){
		if(values == null) values = new Dictionary<string, string>();
	
		GameObject add = AddPiece(styleVarPrefab);
		GameObject titleText = add.transform.GetChild(0).gameObject;
		GameObject inputText = add.transform.GetChild(1).gameObject;
		SetValue(title, defaultValue);
		titleText.GetComponent<Text>().text = title;
		inputText.GetComponent<InputField>().text = defaultValue;
		inputText.GetComponent<InputField>().onValueChange.AddListener( (string value) => { SetValue(title,value); } );
	}
	
	public void AddEmptyAttrib(string title, string defaultValue){
		if(attributes == null) attributes = new List<Attribute>();
		
		Attribute attrib = new Attribute();
		attrib.title = title;
		attrib.value = defaultValue;
		int attribInd = attributes.Count;
		attributes.Add(attrib);
		
		SetAttributeTitle(attribInd,title);
		SetAttributeValue(attribInd,defaultValue);
		
		GameObject add = AddPiece(attribVarPrefab);
		GameObject titleText = add.transform.GetChild(0).gameObject;
		GameObject inputText = add.transform.GetChild(1).gameObject;
		titleText.GetComponent<InputField>().text = title;
		inputText.GetComponent<InputField>().text = defaultValue;
		
		titleText.GetComponent<InputField>().onValueChange.AddListener( (string value) => { SetAttributeTitle(attribInd,value); } );
		inputText.GetComponent<InputField>().onValueChange.AddListener( (string value) => { SetAttributeValue(attribInd,value); } );
	}
	
	public void AddSpriteVar(string title, int defaultValue){
		if(values == null) values = new Dictionary<string, string>();
		
		GameObject add = AddPiece(selectorPrefab);
		GameObject titleText = add.transform.GetChild(0).gameObject;
		GameObject button = add.transform.GetChild(1).gameObject;
		SetValue(title, defaultValue + "");
		titleText.GetComponent<Text>().text = title;
		button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { 
			GameObject selectorMenu = (GameObject)Instantiate(spriteSelector);
			selectorMenu.transform.parent = this.transform.parent;
			selectorMenu.transform.localPosition = Vector3.zero;
			
			SpriteSelector sel = selectorMenu.GetComponent<SpriteSelector>();
			sel.Init(defaultValue, (spriteID) => {
				SetValue(title,spriteID + ""); //When menu closes, set sprite to that 
			});
		} );
	}
	
	private GameObject AddPiece(GameObject prefab){
		GameObject add = (GameObject)Instantiate (prefab);
		float y = 0;
		int i;
		for(i = 0; i < this.transform.childCount; i++){
			y -= ((RectTransform)this.transform.GetChild(i)).sizeDelta.y-1;
			
		}
		add.transform.SetParent(this.transform);
		add.transform.localPosition = new Vector3(0,y,10);
		pieces.Add (add);
		
		//Recentering popup
		float height = Mathf.Abs(y) + ((RectTransform)add.transform).sizeDelta.y;
		Vector3 pos = this.transform.localPosition;
		this.transform.localPosition = new Vector3(pos.x, originY + height/2, pos.z);
		return add;
	}
	
	private void InsertEmptyAttrib(string title, string defaultValue){
		if(attributes == null) attributes = new List<Attribute>();
		
		Attribute attrib = new Attribute();
		attrib.title = title;
		attrib.value = defaultValue;
		int attribInd = attributes.Count;
		attributes.Add(attrib);
		
		SetAttributeTitle(attribInd,title);
		SetAttributeValue(attribInd,defaultValue);
		
		GameObject add = InsertPiece(attribVarPrefab, this.transform.childCount-3);
		GameObject titleText = add.transform.GetChild(0).gameObject;
		GameObject inputText = add.transform.GetChild(1).gameObject;
		titleText.GetComponent<InputField>().text = title;
		inputText.GetComponent<InputField>().text = defaultValue;
		
		titleText.GetComponent<InputField>().onValueChange.AddListener( (string value) => { SetAttributeTitle(attribInd,value); } );
		inputText.GetComponent<InputField>().onValueChange.AddListener( (string value) => { SetAttributeValue(attribInd,value); } );
	}
	
	private GameObject InsertPiece(GameObject prefab, int index){
		// "Keep it simple until you run out of simple" I ran out of simple. Good luck future me.
		GameObject add = (GameObject)Instantiate (prefab);
		float y = 0;
		List<Transform> removed = new List<Transform>();
		for(int i = 0; i <= this.transform.childCount; i++){
			if(i == index+1){
				i--; //Keep loop going
				Transform rem = this.transform.GetChild(i);
				removed.Add(rem);
				rem.SetParent(this.transform.parent); //Temporarily move out of popup
			}
			else if(i != index) //Skip current index cuz not sure?
				y -= ((RectTransform)this.transform.GetChild(i)).sizeDelta.y-1;
			
		}
		
		add.transform.SetParent(this.transform);
		add.transform.localPosition = new Vector3(0,y,10);
		pieces.Add (add);
		y -= ((RectTransform)add.transform).sizeDelta.y-1;
		
		foreach (Transform t in removed){
			t.SetParent(this.transform);
			t.localPosition = new Vector3(0,y,10);
			y -= ((RectTransform)t).sizeDelta.y-1;
		}
		
		//Recentering popup
		float height = Mathf.Abs(y);
		Vector3 pos = this.transform.localPosition;
		this.transform.localPosition = new Vector3(pos.x, originY + height/2, pos.z);
		return add;
	}
	
	public void Close(bool cancel){
		if(closer != null){
			Dictionary<string,string> data = new Dictionary<string, string>();
			if(values != null)
				foreach(string key in values.Keys){
					data[key] = values[key];
				}
			
			if(attributes != null)
				foreach(Attribute a in attributes){
					data[a.title] = a.value;
				}
			
			closer(data, cancel);
		}
		Destroy(this.gameObject);
	}
	
	public void SetValue(string title, string value){
		values[title] = value;
		
		CallChange ();
	}
	
	public void SetAttributeTitle(int attrib, string title){
		Attribute get = attributes[attrib];
		get.title = title;
		attributes[attrib] = get;
		
		CallChange ();
	}
	public void SetAttributeValue(int attrib, string value){
		Attribute get = attributes[attrib];
		get.value = value;
		attributes[attrib] = get;
		
		CallChange ();
	}
	
	private void CallChange(){
		if(changer == null || !ready) return;
		Dictionary<string,string> data = new Dictionary<string, string>();
		if(values != null)
		foreach(string key in values.Keys){
			data[key] = values[key];
		}
		
		if(attributes != null)
		foreach(Attribute a in attributes){
			data[a.title] = a.value;
		}
		
		changer(data);
	}
}
