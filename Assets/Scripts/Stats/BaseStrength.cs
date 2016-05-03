using UnityEngine;
using System.Collections;

public class BaseStrength : BaseStat
{

    public BaseStrength()
    {
        StatName = "Strength";
        StatDescription = "Makes you pound shit harder";
        StatType = StatTypes.STRENTGH;
        StatBaseValue = 0;
        StatModifiedValue = 0;
    }
}