using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Map/Tile Light")]
public class TileLight : MonoBehaviour {
	
	public const float MIN_RANGE = -64.6f;
	public const float MAX_RANGE = -2.0f;
	private const float rangeDiff =  MAX_RANGE-MIN_RANGE;

	[SerializeField]
	[Range(0,1f)]
	private float intensity = 0.6f;
	public float Intensity{
		get { return intensity; }
		set { change = (intensity != value); intensity = value; }
	}
	
	[SerializeField]
	[Range(0,1)]
	private float range = 3f;
	public float Range{
		get { return range; }
		set { change = (range != value); range = value; }
	}
	
	[SerializeField]
	private Color color = new Color(1,0.419f,0);
	public Color Color{
		get { return color; }
		set { change = (color != value); color = value; }
	}
	
	private bool change = false;
	private Light light;

	// Use this for initialization
	void Start () {
		change = true;
		this.light = this.GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		if(change){
			this.light.intensity = this.intensity * 8f;
			Vector3 pos = this.transform.localPosition;
			pos.z = range*rangeDiff + MIN_RANGE;
			this.transform.localPosition = pos;
			this.light.color = this.color;
		}
	}
}
