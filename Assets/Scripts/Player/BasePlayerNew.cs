using UnityEngine;
using System.Collections;
using System;
//11.05.2016, Fabian Wendland

namespace Gen{
	[Serializable]
	public class BasePlayerNew {
		
		public int Strength;
		public int Dexterity;
		public int Intellect;
		public int Luck;
		public int Health;
		public int Wisdom;
		
		public static BasePlayerNew operator +(BasePlayerNew lhs, BasePlayerNew rhs){
			return new BasePlayerNew {
				Strength = lhs.Strength + rhs.Strength,
				Dexterity = lhs.Dexterity + rhs.Dexterity,
				Intellect = lhs.Intellect + rhs.Intellect,
				Luck = lhs.Luck + rhs.Luck,
				Health = lhs.Health + rhs.Health,
				Wisdom = lhs.Wisdom + rhs.Wisdom,
			};
		}
		public static BasePlayerNew Interpolate(BasePlayerNew a, BasePlayerNew b, float f) {
            f = Mathf.Clamp01(f);
            return new BasePlayerNew {
                Strength = (int)Mathf.Lerp(a.Strength, b.Strength, f),
                Dexterity = (int)Mathf.Lerp(a.Dexterity, b.Dexterity, f),
                Intellect = (int)Mathf.Lerp(a.Intellect, b.Intellect, f),
                Luck = (int)Mathf.Lerp(a.Luck, b.Luck, f),
            };
	}
}
}
