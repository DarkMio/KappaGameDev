using UnityEngine;
using System.Collections;

public class BaseMana : BaseStat
{

    public BaseMana()
    {
        StatName = "Mana";
        StatDescription = "More of that juicy stuff";
        StatType = StatTypes.MANA;
        StatBaseValue = 0;
        StatModifiedValue = 0;
    }
}
