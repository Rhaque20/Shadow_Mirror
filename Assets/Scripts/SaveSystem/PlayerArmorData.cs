using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerArmorData
{
    public string name;
    public float[] statval;// Index 0 is main stat
    // 0 = HP 1 = ATK 2 = DEF 3 = Potency 4 = Resistance 5 = C. Rate 6 = C. DMG 7 = SP Gain 8 = AetherPower
    public int[] stat;
    public int[] statrarity;//Follows similar grades
    public float[] upgrades;
    public int setid = 0;
    public int slot;//0 = Helmet 1 = Chestplate 2 = Boots 3 = Necklace 4 = Ring 5 = Belt 6 = Weapon
    public int level = 1;
    public int trigger = 0;
    public int grade = 0;

    public int enhancelevel;
    public float totalXP;
    public string guid;

    public PlayerArmorData(Armor a)
    {
        //Debug.Log("Transcribing armor index "+a.setid+" from "+a.name);
        setid = a.setid;
        slot = a.PieceInt;
        level = a.level;
        grade = a.rarity;
        name = a.name;
        
        trigger = a.TriggerInt;

        statval = a.statval;
        stat = a.stat;
        statrarity = a.statrarity;
        upgrades = a.upgrades;
        enhancelevel = a.enhancelevel;
        totalXP = a.totalXP;
        guid = a.guid;

    }
}
