using UnityEngine;
using System.Collections;


/* Probably only useful for orthographic cameras,
 * draws a debug grid based on material in the background.
 * 
 */
#if UNITY_EDITOR     // run only in edito
[ExecuteInEditMode]  // run in editor in the first place
public class DebugCamera : MonoBehaviour {

    private Camera cam;
    private RenderTexture renderTex;
    private Rect textureSize;
    private GameObject quad;
    private Texture quadTex;
    
    public Material textureTile;
    public int distanceFromCamera = 1000;
    public Rect quadSize = new Rect(0, 0, 100, 100);
	
    // Use this for initialization
	void Start () {
	    cam = GetComponent<Camera>();
        renderTex = cam.targetTexture;
	    textureSize = new Rect(0, 0, renderTex.width, renderTex.height);
        if(quad == null) {
            quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.SetActive(true);
            ScaleAndRotate(quad);
        }
    }
	
	void OnPreRender () {
        if (quad == null) { // rebuild if quad is missing
            Debug.Log("Rebuilding Debug Camera...");
            Start();
        }
	    ScaleAndRotate(quad);
        TextureTiling();
	}
    
    // I believe this could need some cleanup.
    private void ScaleAndRotate(GameObject child) {
        Vector3 totalScale = child.transform.lossyScale;
        Vector3 relativeScale = child.transform.localScale;
        Vector3 scaleMultiplier = new Vector3(
                totalScale.x / relativeScale.x,
                totalScale.y / relativeScale.y,
                totalScale.z / relativeScale.z
            );
        Vector2 fitSize = new Vector3(quadSize.width / scaleMultiplier.x, quadSize.height / scaleMultiplier.y);
        child.transform.rotation = cam.transform.rotation;
        child.transform.localScale = new Vector3(fitSize.x, fitSize.y, relativeScale.z);
        child.transform.position = cam.transform.forward * distanceFromCamera;
    }
    
    private void TextureTiling() {
        Renderer r = quad.GetComponent<Renderer>();
        r.sharedMaterial = textureTile;
        Vector3 size = quad.transform.lossyScale;
        r.sharedMaterial.mainTextureScale = new Vector2(size.x / textureTile.mainTexture.width, size.y / textureTile.mainTexture.height);
    }
}
#endif