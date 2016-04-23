using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public abstract class MenuTrigger : MonoBehaviour {
	private float m_triggerDistance;
	public float triggerDistance {
		get {
			return m_triggerDistance;
		}
		set {
			m_triggerDistance = value;
		}
	}
	
	public abstract void TriggerMenu();
}