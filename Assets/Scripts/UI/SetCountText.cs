using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[AddComponentMenu("Scripts/UI/Set Item Count Text")]
public class SetCountText : MonoBehaviour {

	public Text text;

	public void SetCount(int count){
		count = Mathf.Min(count,999);
		text.enabled = (count != 1);
		text.text = "~" + count;
	}
}
