using UnityEngine;
using System.Collections;

public class TrashCameraMover : MonoBehaviour {
     public Vector3 pos1 = new Vector3(-35,135,-9);
     public Vector3 pos2 = new Vector3(35,160,-9);
     public float speed = 1.0f;
 
     void Update() {
         transform.position = Vector3.Lerp (pos1, pos2, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
     }
}
