using UnityEngine;

/** Mio, 13.04.2016
 * Snaps a GameObject to the grid by performing floor operations
 * Enabled in EditorMode too, so performing maintenance tasks is easy
 * It adds a half to the world positioning if the texture size is odd,
 * since it could sit exactly between two points, which generated nasty multi-lining in rendering
 */
[ExecuteInEditMode]
public class UnitSnap : MonoBehaviour {
    public enum EAxisOptions {X, Y, Z, XY, XZ, YZ, XYZ};
    public EAxisOptions axisSnap = EAxisOptions.XY;
    private Transform _objTransform;
    private bool _isRendered = false;
    private bool _oddHeight, _oddWidth; // Texture W/H
    
	void Start () {
	    _objTransform = GetComponent<Transform>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Rect size;
        if(sr != null) { // if it's a sprite
            _isRendered = true;
            size = sr.sprite.rect;
        } else { // We got no sprite renderer, so we may have a regular renderer?
            Renderer r = GetComponent<Renderer>();
            if(sr != null) { // well, we have one
                _isRendered = true;
                size = new Rect(r.sharedMaterial.mainTexture.width, r.sharedMaterial.mainTexture.height, 0, 0);
            } else { // we don't have one, so we do not care about further positioning
                return;
            }
        }
        _oddWidth = size.width % 2 == 1; // tex / sprite width is odd 
        _oddHeight = size.height % 2 == 1; // tex / sprite height is odd
	}
	
	void Update () {
        var a = axisSnap;
        Vector3 pos = _objTransform.position;
        // Please don't do that at home - but it works best with the unity editor.
        if (a == EAxisOptions.X || a == EAxisOptions.XY || a == EAxisOptions.XZ || a == EAxisOptions.XYZ) {
            pos.x = Mathf.RoundToInt(pos.x);
        }
        if (a == EAxisOptions.Y || a == EAxisOptions.XY || a == EAxisOptions.YZ || a == EAxisOptions.XYZ) {
            pos.y = Mathf.RoundToInt(pos.y);
        }
        if (a == EAxisOptions.Z || a == EAxisOptions.XZ || a == EAxisOptions.YZ || a == EAxisOptions.XYZ) {
            pos.z = Mathf.RoundToInt(pos.z); 
        }
        if(_isRendered) { // if it's rendered and
            if(_oddWidth) { // width is odd, then
                pos.x += 0.5f; // add a bit a half to positioning
            }
            if(_oddHeight) { // likewise here
                pos.y += 0.5f;
            }
        }
        _objTransform.position = pos;
	}
}