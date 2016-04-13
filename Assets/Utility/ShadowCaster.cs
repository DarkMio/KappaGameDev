using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ShadowCaster : MonoBehaviour {
	void Start () {
        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
	}
}
