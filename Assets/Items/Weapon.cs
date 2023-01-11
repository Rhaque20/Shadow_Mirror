using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon: Equipment
{
    /**
    +-----+-----+-----+-----+-----+-----+-----+-----+-----+
    |  HP | ATK | DEF | POT | RES | CRI | CDMG|SPGAI| SP  |
    +-----+-----+-----+-----+-----+-----+-----+-----+-----+
    **/
    public enum WeaponType{Dagger, Mace, Lance, Longsword, Catalyst};
    public WeaponType wp;
    public int[] stat = new int[2];//Has Main and SubStat
    public float[] statval = new float[2];// Index 0 is main stat
    // 0 = HP 1 = ATK 2 = DEF 3 = Potency 4 = Resistance 5 = C. Rate 6 = C. DMG 7 = SP Gain
    public string weaponSkill;
    public int PieceInt = 6;
    public float upgrades = 0f;

    public int WeaponTypeInt
    {
        get
        {
            string[] s = Enum.GetNames(typeof(WeaponType));

            return Array.IndexOf(s,wp.ToString());
        }
        set
        {
            string[] s = Enum.GetNames(typeof(WeaponType));

            Enum.TryParse(s[value],out WeaponType wp);
        }
    }


    public Weapon(PlayerWeaponData pwd)
    {
        level = pwd.level;
        rarity = pwd.grade;
        name = pwd.name;

        TriggerInt = pwd.trigger;
  
        stat = pwd.stat;
        statval = pwd.statval;
        upgrades =pwd.upgrades;
        enhancelevel = pwd.enhancelevel;
        totalXP = pwd.totalXP;

        WeaponTypeInt = pwd.weaponType;
    }
    

}