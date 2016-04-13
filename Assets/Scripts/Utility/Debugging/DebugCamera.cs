using UnityEngine;

/** Mio, 13.04.2016
 * Probably only useful for orthographic cameras,
 * draws a debug grid based on material in the background. 
 */
#if UNITY_EDITOR     // run only in editor
[ExecuteInEditMode]  // run in editor in the first place
public class DebugCamera : MonoBehaviour {

    private Camera _cam;
    private GameObject _quad;
    
    public Material textureTile;
    public int distanceFromCamera = 1000;
    public Rect quadSize = new Rect(0, 0, 100, 100);
	
    
	void Start () {
	    _cam = GetComponent<Camera>();
        if(_quad == null) {
            _quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            _quad.SetActive(true);
            ScaleAndRotate(_quad);
        }
    }
	
	void OnPreRender () {
        if (_quad == null) { // rebuild if quad is missing
            Debug.Log("Rebuilding Debug Camera...");
            Start();
        }
	    ScaleAndRotate(_quad);
        TextureTiling();
	}
    
    /**
     * Scales and rotates a given child (most likely the camera)
     */
    private void ScaleAndRotate(GameObject child) {
        // World Size
        Vector3 totalScale = child.transform.lossyScale;
        // Its relative scale (based off it's from a child)
        Vector3 relativeScale = child.transform.localScale;
        // Determines the scaling to keep relative sizes intact
        Vector3 scaleMultiplier = new Vector3(
                totalScale.x / relativeScale.x,
                totalScale.y / relativeScale.y,
                totalScale.z / relativeScale.z
            );
        // Determine X/Y Scaling to span over the camera properly
        Vector2 fitSize = new Vector2(quadSize.width / scaleMultiplier.x, quadSize.height / scaleMultiplier.y);
        // Override childs rotation with cams rotation (billboarding)
        child.transform.rotation = _cam.transform.rotation;
        // Scale X/Y Axis appropiately to span over the camera
        child.transform.localScale = new Vector3(fitSize.x, fitSize.y, relativeScale.z);
        // And move the camera far enough away
        child.transform.position = _cam.transform.forward * distanceFromCamera;
    }
    
    /**
     * Tiles the assigned Debug Materials Texture to have a pixel perfect view on it
     */
    private void TextureTiling() {
        if(textureTile == null) {
            Debug.Log("Cannot tile debug texture as it is null.");
            return;
        }
        Renderer r = _quad.GetComponent<Renderer>();
        // Writing it into shared material stops leaking materials into the void
        r.sharedMaterial = textureTile;
        // Total size of render Quad
        Vector3 size = _quad.transform.lossyScale;
        // Adjust tiling to render quad size
        r.sharedMaterial.mainTextureScale = new Vector2(size.x / textureTile.mainTexture.width, size.y / textureTile.mainTexture.height);
    }
}
#endif