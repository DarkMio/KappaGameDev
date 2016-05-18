using UnityEngine;
using System.Collections;

//15.05.2016, Fabian Wendland

namespace Item{
    
    public class ItemAffix : ScriptableObject {
        
        public ItemStats Level1BonusStats;
        public ItemStats Level100BonusStats;
        public string Prefix;
        public string Suffix;
        
        public ItemStats GetItemStatsForLevel(int level){
            int clampedLevel = Mathf.Clamp(level, 1, 100);
            float levelNormal = Mathf.Pow((clampedLevel - 1) / 99.0f, 1.2f);
            return ItemStats.Interpolate(Level1BonusStats, Level100BonusStats, levelNormal);
        }
        public static ItemAffix Get(string id ){
            return Resources.Load<ItemAffix>("ItemAffixes/" + id);
        }
        public static ItemAffix GetRandom(){
            var list = Resources.LoadAll<ItemAffix>("ItemAffixes");
            return list[Random.Range(0, list.Length)];
        }
    }
}