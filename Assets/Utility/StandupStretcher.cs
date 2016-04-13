using UnityEngine;
using System.Collections;

/**
 * The Standup-Stretcher is planned to stretch standing up sprites perfectly for
 * angled orthographic cameras. This way we can stretch our sprites to stay in
 * pixel perfect alignment while taking part of the world space.
 *
 * Note to future self: Maybe the Camera can take care of that next time.
 *                      at this point all standing up sprites have to be aligned.
 */
#if UNITY_EDITOR  // So we're _only_ adjusting in editor view and save us some time.
[ExecuteInEditMode]
public class StandupStretcher : MonoBehaviour {
    private Camera renderCamera;
    public GameObject[] sprites;
    public Transform trashVar;
    private float m_angle;
    private float m_radAngle;
    private float angle {
        get {
            return m_angle;
        }
        set {
            m_angle = value;
            m_radAngle = DegToRad(value);
        }
    }
    private float radAngle {
        get {
            return m_radAngle;
        }
        set {
            m_radAngle = value;
            m_angle = RadToDeg(value);
        }
    }
    
    // Use this for initialization
	void Start () {
	    // main = Camera.main;
        trashVar = transform;
        renderCamera = GetComponent<Camera>();
        angle = Quaternion.Angle(Quaternion.identity, transform.rotation);
	}
	
	// Update is called once per frame
	void Update() {
        float compAngle = Quaternion.Angle(Quaternion.identity, transform.rotation);
        float radAngle = DegToRad(compAngle);
        if(compAngle != angle) {
            angle = compAngle;
            Debug.Log("@" + Time.realtimeSinceStartup + ": Hello, I don't like you - compAngle: " + 1/Mathf.Cos(radAngle));

            foreach(GameObject sprite in sprites) {
                var standTransform = sprite.GetComponent<Transform>();
                var scal = standTransform.localScale;
                standTransform.localScale = new Vector3(scal.x, 1 / Mathf.Cos(radAngle), scal.z);
            }
        }
	}
    
    private float DegToRad(float a) {
        return Mathf.PI / 180 * a;
    }
    
    private float RadToDeg(float a) {
        return a / 180 * Mathf.PI;
    }
}
#endif
