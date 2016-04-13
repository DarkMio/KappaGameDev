using UnityEngine;

/** Mio, 13.04.2016
 * The Standup-Stretcher is planned to stretch standing up sprites perfectly for
 * angled orthographic cameras. This way we can stretch our sprites to stay in
 * pixel perfect alignment while taking part of the world space.
 */
[ExecuteInEditMode]
public class StandupStretcher : MonoBehaviour {
    public GameObject[] sprites;
    public Transform trashVar;
    private float m_angle;
    private float m_radAngle;
    private float _angle { // convert from deg to rad - synced
        get {
            return m_angle;
        }
        set {
            m_angle = value;
            m_radAngle = DegToRad(value);
        }
    }
    private float _radAngle { // convert from rad to deg - synced
        get {
            return m_radAngle;
        }
        set {
            m_radAngle = value;
            m_angle = RadToDeg(value);
        }
    }
    
	void Start () {
	    // main = Camera.main;
        trashVar = transform;
        _angle = Quaternion.Angle(Quaternion.identity, transform.rotation);
	}
	
	void Update() {
        float compAngle = Quaternion.Angle(Quaternion.identity, transform.rotation);
        float radAngle = DegToRad(compAngle);
        if(compAngle != _angle) { // if the angle has changed
            _angle = compAngle;
            foreach(GameObject sprite in sprites) { // then do all calculations on each sprite
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
// #endif