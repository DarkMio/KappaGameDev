using UnityEngine;

/** Mio, 13.04.2016
 * This class overwrites a hidden feature needed to enable shadowcasting from sprites
 * Attach this to any GameObject with a renderer.
 * Can be accessed otherwise by: Inspector -> Debug -> Cast Shadows
 */
[ExecuteInEditMode]
public class ShadowCaster : MonoBehaviour {
    
	void Start () {
        Renderer r = GetComponent<Renderer>();
        if(r != null) { 
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        } else {
            Debug.Log("Renderer is missing from GameObject - not a rendered Object?");
        }
	}
}
