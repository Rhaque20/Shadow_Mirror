using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Armor", menuName = "Armor")]
public class Armor: Equipment
{
    public enum Sets{BrightStar,DarkStar};
    public float[] statval = new float[1];// Index 0 is main stat
    // 0 = HP 1 = ATK 2 = DEF 3 = Potency 4 = Resistance 5 = C. Rate 6 = C. DMG 7 = SP Gain 8 = AetherPower
    public int[] stat = new int[1];
    public int[] statrarity = new int[1];//Follows similar grades
    public float[] upgrades; // Keep track of the increments for each substat
    //public int setid = 0;
    public Piece slot;
    public Sets set;
    public string guid;

    public int setid
    {
        get
        {
            //string[] s = Enum.GetNames(typeof(Sets));
            //Debug.Log("Accessing index : "+Array.IndexOf(s,set.ToString()));

            //return Array.IndexOf(s,set.ToString());
            return (int)set;
        }
        set
        {
            string[] s = Enum.GetNames(typeof(Equipment.Trigger));

            Enum.TryParse(s[value],out Equipment.Trigger active);
        }
    }

    public int PieceInt
    {
        get
        {
            switch(slot)
            {
                case Piece.Chestplate:
                    return 1;
                case Piece.Boots:
                    return 2;
                case Piece.Necklace:
                    return 3;
                case Piece.Ring:
                    return 4;
                case Piece.Belt:
                    return 5;
                default:
                    return 0;
            }
        }
        set
        {
            switch(value)
            {
                case 1:
                    slot = Piece.Chestplate;
                    break;
                case 2:
                    slot = Piece.Boots;
                    break;
                case 3:
                    slot = Piece.Necklace;
                    break;
                case 4:
                    slot = Piece.Ring;
                    break;
                case 5:
                    slot = Piece.Belt;
                    break;
                default:
                    slot = Piece.Helmet;
                    break;
            }
        }
    }

    public string StatName(int stat)
    {
        switch(stat)
        {
            case 0:
                return "Health";
            case 1:
                return "Attack";
            case 2:
                return "Defense";
            case 3:
                return "Potency";
            case 4:
                return "Resistance";
            case 5:
                return "Critical Rate";
            case 6:
                return "Critical Damage";
            default:
                return "SP Gain";
        }

    }

    public void DisplaySubstat()
    {
        for(int i = 0; i < stat.Length; i++)
        {
            Debug.Log("Substat "+(i+1)+":"+StatName(stat[i])+" "+statval[i]);
        }
    }

    public void intToPiece(int i)
    {
        switch(i)
        {
            case 1:
                slot = Piece.Chestplate;
                break;
            case 2:
                slot = Piece.Boots;
                break;
            case 3:
                slot = Piece.Necklace;
                break;
            case 4:
                slot = Piece.Ring;
                break;
            case 5:
                slot = Piece.Belt;
                break;
            default:
                slot = Piece.Helmet;
                break;
        }

    }

    public void intToGrade(int i)
    {
        switch(i)
        {
            case 2:
                grade = Grade.Uncommon;
                break;
            case 3:
                grade = Grade.Rare;
                break;
            case 4:
                grade = Grade.Elite;
                break;
            case 5:
                grade = Grade.Legendary;
                break;
            case 6:
                grade = Grade.Phantasmal;
                break;
            default:
                grade = Grade.Common;
                break;
        }
        
    }


    public Armor(Piece p, Grade g, float[] sval, int[] s, int[] srare, float[] u, string n, int l,Sets setname)
    {
        slot = p;
        grade = g;
        statval = sval;
        stat = s;
        statrarity = srare;
        upgrades = u;
        name = n;
        level = l;
        set = setname;
        totalXP = 0f;
        enhancelevel = 0;
        set = setname;
        guid = Guid.NewGuid().ToString();
        //Roll();
    }

    public Armor(PlayerArmorData pad)
    {
        intToPiece(pad.slot);
        intToGrade(pad.grade);
        statval = pad.statval;
        stat = pad.stat;
        statrarity = pad.statrarity;
        upgrades = pad.upgrades;
        name = pad.name;
        level = pad.level;
        totalXP = pad.totalXP;
        enhancelevel = pad.enhancelevel;
        setid = pad.setid;
        guid = pad.guid;
    }
}