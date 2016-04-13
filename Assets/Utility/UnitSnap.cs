using UnityEngine;
using UnityEditor;
/**
 * Snaps a GameObject to the grid
 */
 [ExecuteInEditMode]
public class UnitSnap : MonoBehaviour {
    public enum EAxisOptions {X, Y, Z, XY, XZ, YZ, XYZ};
    public EAxisOptions axisSnap = EAxisOptions.XY;
    private Transform objTransform;
    
    
	// Use this for initialization
	void Start () {
	    objTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        var a = axisSnap;
        Vector3 pos = objTransform.position;
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
        objTransform.position = pos;
	}
}