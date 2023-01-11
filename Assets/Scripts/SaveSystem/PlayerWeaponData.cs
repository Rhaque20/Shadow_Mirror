using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerWeaponData
{
    //public enum WeaponType{Dagger, Mace, Lance, Longsword, Catalyst};
    public int wp;//Weapon
    public int[] stat = new int[2];//Has Main and SubStat
    public float[] statval = new float[2];// Index 0 is main stat
    // 0 = HP 1 = ATK 2 = DEF 3 = Potency 4 = Resistance 5 = C. Rate 6 = C. DMG 7 = SP Gain
    public string name;
    public int trigger;
    public int level;
    public int grade;
    public int slot;

    public float upgrades;
    public int enhancelevel;
    public float totalXP;
    public int weaponType;

    public PlayerWeaponData(Weapon a)
    {
        //slot = a.PieceInt;
        level = a.level;
        grade = a.rarity;
        name = a.name;
        
        trigger = a.TriggerInt;

        statval = a.statval;
        stat = a.stat;
        upgrades = a.upgrades;
        enhancelevel = a.enhancelevel;
        totalXP = a.totalXP;

        weaponType = a.WeaponTypeInt;

    }
}
