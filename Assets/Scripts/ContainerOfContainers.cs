using UnityEngine;
using System.Collections;

public class ContainerOfContainers : MonoBehaviour {

	public PremadeContainer[] containers;

	public PremadeContainer this[string key] {
		get {
			foreach(PremadeContainer con in containers){
				if(con.Name() == key) return con;
			}
			return null;
		}
	}
}
