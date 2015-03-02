using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class FileTabButton : MonoBehaviour {

	public EditorUI invMan;
	public Text text;
	
	private int file;
	
	public void Init(EditorUI invMan, int num){
		this.invMan = invMan;
		this.SetNumber(num);
	}
	
	public void SetNumber(int num){
		text.text = "" + (num);
		file = num;
	}
	
	public void OnClick(){
		invMan.SwitchToMap(file);
	}
}
