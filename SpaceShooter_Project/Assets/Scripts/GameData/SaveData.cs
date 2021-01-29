using System;
using System.Collections.Generic;

[Serializable]
public struct SaveData
{
    public int coins;

    public int stars;

    public int lastLevelBeat;

    public int healthLevel;

    public int shieldArmor;

    public bool isVoidPickupEnable;

    public bool isTimePickupEnable;

    public bool isRepairPickupEnable;

    public bool isBombAbilityEnable;

    public int speedLevel;

    public int shotLevel;
}
