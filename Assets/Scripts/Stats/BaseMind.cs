using UnityEngine;
using System.Collections;

public class BaseMind : BaseStat
{

    public BaseMind()
    {
        StatName = "Mind";
        StatDescription = "Mind over matter or something like that";
        StatType = StatTypes.MIND;
        StatBaseValue = 0;
        StatModifiedValue = 0;
    }
}
