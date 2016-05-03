using UnityEngine;
using System.Collections;

public class BaseIntellect : BaseStat
{

    public BaseIntellect()
    {
        StatName = "Intellect";
        StatDescription = "Makes you doubt your decisions in life twice as much";
        StatType = StatTypes.INTELLECT;
        StatBaseValue = 0;
        StatModifiedValue = 0;
    }
}
