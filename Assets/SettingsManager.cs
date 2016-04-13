using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SettingsManager : MonoBehaviour {
    public Material renderTexture;
    public FilterMode filtering = FilterMode.Point;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    // Gets called if there was any input in the inspector fields
    void OnValidate() {
        SetRenderFiltering(filtering);
    }
    
    public void SetRenderFiltering(FilterMode mode) {
        renderTexture.mainTexture.filterMode = mode;
    }
}
