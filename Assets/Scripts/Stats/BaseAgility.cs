using UnityEngine;
using System.Collections;

public class BaseAgility : BaseStat
{

    public BaseAgility()
    {
        StatName = "Agility";
        StatDescription = "Makes you shithead stealing stuff easier";
        StatType = StatTypes.AGILITY;
        StatBaseValue = 0;
        StatModifiedValue = 0;
    }
}
