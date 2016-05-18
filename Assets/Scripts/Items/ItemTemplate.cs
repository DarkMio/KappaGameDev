using UnityEngine;
using System.Collections;

namespace Item {
	public class ItemTemplate : ScriptableObject {
		
		public ItemStats Level1Stats;
		public ItemStats Level100Stats;
		public string[] Names;
		
		
		public string GetNameForLevel(int level){
			int clampedLevel = Mathf.Clamp(level, 1, 100);
			int tfLevel = Mathf.Clamp((int)(((clampedLevel - 1) / 99.0f) * Names.Length), 0, Names.Length);
			return Names[tfLevel];
		}
		
		public ItemStats GetStatsForLevel(int level){
			int clampedLevel = Mathf.Clamp(level, 1, 100);
			float levelNormal = Mathf.Pow((clampedLevel - 1) / 99.0f, 1.2f);
			return ItemStats.Interpolate(Level1Stats, Level100Stats, levelNormal);
		}
		
		public static ItemTemplate Get(ItemType type){
			return Resources.Load<ItemTemplate>("ItemTemplates/" + type.ToString());
		}
	}
}
