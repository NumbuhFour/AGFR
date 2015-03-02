using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[AddComponentMenu("Scripts/LevelEditor/Scrollbar Button")]
public class ScrollbarButton : MonoBehaviour {

	public Scrollbar scrollbar;
	
	public void Scroll(float amt){
		this.scrollbar.value += amt;
	}
}
