using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[AddComponentMenu("Scripts/UI/Substitute Text For Key")]
public class SubstituteTextForKey : MonoBehaviour {

	public string inputAxis;
	
	private string substitute = "$key";
	// Use this for initialization
	void Start () {
		//TODO: One day, unity better have a way of getting the bound key to anis
		//Text t = gameObject.GetComponent<Text>();
		//t.text = t.text.Replace(substitute, Input. 
	}
}
