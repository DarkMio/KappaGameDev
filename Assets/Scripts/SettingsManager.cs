using UnityEngine;

/** Mio, 13.04.2016
 * SettingsManager keeps track and sets settings on change.
 * It holds and stores a full data set of all user configurations
 * Currently does not work automatically on changes ingame, but
 * works fine in editor.
 * There seems to be no good way of observing property changes (???)
 */
[ExecuteInEditMode]
public class SettingsManager : MonoBehaviour {
    public Material renderTexture;
    public FilterMode filtering = FilterMode.Point;

    void Start() {
        OnValidate();
    }
    
    void OnValidate() {
        SetRenderFiltering(filtering);
    }
    
    
    public void SetRenderFiltering(FilterMode mode) {
        renderTexture.mainTexture.filterMode = mode;
    }
}
