using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[AddComponentMenu("Scripts/LevelEditor/Popup")]
public class EditorPopup : MonoBehaviour {

	public delegate void OnClose(Dictionary<string,string> data, bool cancelled);

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
	
	private List<GameObject> pieces;
	
	public Dictionary<string, string> values; //For preset attributes (x, y, width, etc)
	public List<Attribute> attributes; //For attribute entries with changing name

	private OnClose closer;

	private float originY;
	// Use this for initialization
	void Start () {
		originY = 0;//this.transform.localPosition.y;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void InitEmpty(OnClose closer){
		this.closer = closer;
		pieces = new List<GameObject>();
		GameObject top = AddPiece(topPrefab);
		top.transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { Close (true); } );
	}
	public void End(){
		AddPiece(bottomPrefab);
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
		titleText.GetComponent<Text>().text = title;
		inputText.GetComponent<InputField>().text = defaultValue;
		inputText.GetComponent<InputField>().onValueChange.AddListener( (string value) => { SetValue(title,value); } );
		SetValue(title, defaultValue);
	}
	
	public void AddEmptyAttrib(string title, string defaultValue){
		if(attributes == null) attributes = new List<Attribute>();
		
		Attribute attrib = new Attribute();
		attrib.title = title;
		attrib.value = defaultValue;
		int attribInd = attributes.Count;
		attributes.Add(attrib);
		
		
		GameObject add = AddPiece(attribVarPrefab);
		GameObject titleText = add.transform.GetChild(0).gameObject;
		GameObject inputText = add.transform.GetChild(1).gameObject;
		titleText.GetComponent<InputField>().text = title;
		inputText.GetComponent<InputField>().text = defaultValue;
		
		titleText.GetComponent<InputField>().onValueChange.AddListener( (string value) => { SetAttributeTitle(attribInd,value); } );
		inputText.GetComponent<InputField>().onValueChange.AddListener( (string value) => { SetAttributeValue(attribInd,value); } );
	}
	
	private GameObject AddPiece(GameObject prefab){
		GameObject add = (GameObject)Instantiate (prefab);
		float y = 0;
		for(int i = 0; i < this.transform.childCount; i++){
			y -= ((RectTransform)this.transform.GetChild(i)).sizeDelta.y;
			
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
	
	public void Close(bool cancel){
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
		Destroy(this.gameObject);
	}
	
	public void SetValue(string title, string value){
		values[title] = value.ToLower();
	}
	
	public void SetAttributeTitle(int attrib, string title){
		Attribute get = attributes[attrib];
		get.title = title.ToLower();
		attributes[attrib] = get;
	}
	public void SetAttributeValue(int attrib, string value){
		Attribute get = attributes[attrib];
		get.value = value.ToLower();
		attributes[attrib] = get;
	}
}
