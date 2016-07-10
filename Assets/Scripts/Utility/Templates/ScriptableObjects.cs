using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;

namespace Item {
	public class ScriptableObjects {

		public static void CreateAsset<T>() where T : ScriptableObject {
#if UNITY_EDITOR
            T asset = ScriptableObject.CreateInstance<T>();

			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (string.IsNullOrEmpty(path)){
				path = "Assets";
			}
			else if(!string.IsNullOrEmpty(Path.GetExtension(path))){
				path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
			}

			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

			AssetDatabase.CreateAsset(asset, assetPathAndName);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;
#endif
        }
#if UNITY_EDITOR
        [MenuItem("Assets/Create/ItemTemplate")]
		public static void ItemTemplate(){
			CreateAsset<ItemTemplate>();
		}
		[MenuItem("Assets/Create/ItemAffix")]
		public static void ItemAffix() {
			CreateAsset<ItemAffix>();
		}
#endif
	}
}
