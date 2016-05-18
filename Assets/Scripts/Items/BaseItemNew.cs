using UnityEngine;
using System.Collections;

//11.05.2016, Fabian Wendland

public class BaseItemNew : ScriptableObject {
	
	[SerializeField] string _name;
	
	public string Name {
		get {
			return _name;
		}
	}
}