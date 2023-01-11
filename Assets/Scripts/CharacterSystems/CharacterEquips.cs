using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterEquips : MonoBehaviour
{
    [SerializeField]private Armor[] equip = new Armor[6];
    [SerializeField]private Weapon activeWeapon;
    public bool[] triggers = new bool[Enum.GetNames(typeof(Equipment.Trigger)).Length];
    public float[] totalStats = new float[8];
    public float flatHP = 0f, flatATK = 0f, flatDEF = 0f;

    public Armor[] armors
    {
        get{return equip;}
    }

    public Weapon a_Weapon
    {
        get{return activeWeapon;}
    }

    public void processStats(Armor a)
    {
        if (a.PieceInt == 0)
        {
            flatHP = a.statval[0];
        }
        else if (a.PieceInt == 1)
        {
            flatDEF = a.statval[0];
        }
        else
        {
            if (a.stat[0] < totalStats.Length)
            {
                totalStats[a.stat[0]] += a.statval[0];
            }
        }

        for (int i = 1; i < a.statval.Length; i++)
        {
            if (a.stat[i] < totalStats.Length)
                totalStats[a.stat[i]] += a.statval[i];
        }
    }

    public void FillLoadOut(PlayerEquipData ped)
    {
        for (int i = 0; i < 6; i++)
        {
            if (ped.armorLoadOut[i] != null)
            {
                equip[i] = new Armor(ped.armorLoadOut[i]);
                if (string.Compare(equip[i].name,"") != 0)
                {
                    processStats(equip[i]);
                }
            }
        }

        if (ped.weaponLoadOut != null)
            activeWeapon = new Weapon(ped.weaponLoadOut);
    }

    public CharacterEquips(PlayerEquipData ped)
    {
        
    }
}
