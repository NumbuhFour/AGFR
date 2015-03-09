using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[AddComponentMenu("Scripts/LevelEditor/Set Image To Tile Atlas")]
public class SetImageToTileAtlas : MonoBehaviour {
	
	[SerializeField]
	private Texture2D tileAtlas;
	public Texture2D TileAtlas {
		get { return tileAtlas; }
		set {
			tileAtlas = value;
			RefreshAtlas();
		}
	}

	private Image image;
	// Use this for initialization
	void Awake () {
		RefreshAtlas();
	}
	
	public void RefreshAtlas(){
		if(image == null) image = this.GetComponent<Image>();
		
		if(tileAtlas != null){
			image.sprite = Sprite.Create(this.tileAtlas, 
										 new Rect(0,0,this.tileAtlas.width, this.tileAtlas.height), 
										 new Vector2(0.5f,0.5f));
			((RectTransform)image.transform).sizeDelta = new Vector2(this.tileAtlas.width, this.tileAtlas.height);
		}
		else 
			image.sprite = null;
	}
}
