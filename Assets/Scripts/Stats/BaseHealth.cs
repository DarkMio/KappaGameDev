using UnityEngine;
using System.Collections;

public class BaseHealth : BaseStat
{

    public BaseHealth()
    {
        StatName = "Health";
        StatDescription = "Makes you live longer";
        StatType = StatTypes.HEALTH;
        StatBaseValue = 0;
        StatModifiedValue = 0;
    }
}
