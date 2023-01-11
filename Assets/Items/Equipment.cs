using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[CreateAssetMenu(fileName = "New Equipment", menuName = "Equipment")]
public class Equipment: Item
{
    public enum Piece {Helmet,Chestplate,Boots,Necklace,Ring,Belt,Weapon};
    public enum Trigger{Passive,SkillCast,PreDMG,PostDMG,Dodge,Debuff,Buff,NA,onCrit,onDMGdealt,onBattleStart,onBattleEnd};
    public int level = 1;
    public Trigger active = Trigger.Passive;
    public int enhancelevel = 0;
    public float totalXP = 0f;

    public int TriggerInt
    {
        get
        {
            // switch(active)
            // {
            //     case Trigger.SkillCast:
            //         return 1;
            //     case Trigger.PreDMG:
            //         return 2;
            //     case Trigger.PostDMG:
            //         return 3;
            //     case Trigger.Dodge:
            //         return 4;
            //     case Trigger.Debuff:
            //         return 5;
            //     case Trigger.Buff:
            //         return 6;
            //     case Trigger.NA:
            //         return 7;
            //     case Trigger.onCrit:
            //         return 8;
            //     case Trigger.onDMGdealt:
            //         return 9;
            //     case Trigger.onBattleStart:
            //         return 10;
            //     case Trigger.onBattleEnd:
            //         return 11;
            //     default:
            //         return 0;
            // }
            string[] s = Enum.GetNames(typeof(Equipment.Trigger));

            return Array.IndexOf(s,active.ToString());
        }

        set
        {
            // switch(value)
            // {
            //     case 1:
            //         active = Trigger.SkillCast;
            //         break;
            //     case 2:
            //         active = Trigger.PreDMG;
            //         break;
            //     case 3:
            //         active = Trigger.PostDMG;
            //         break;
            //     case 4:
            //         active = Trigger.Dodge;
            //         break;
            //     case 5:
            //         active = Trigger.Debuff;
            //         break;
            //     case 6:
            //         active = Trigger.Buff;
            //         break;
            //     case 7:
            //         active = Trigger.NA;
            //         break;
            //     case 8:
            //         active = Trigger.onCrit;
            //         break;
            //     case 9:
            //         active = Trigger.onDMGdealt;
            //         break;
            //     case 10:
            //         active = Trigger.onBattleStart;
            //         break;
            //     case 11:
            //         active = Trigger.onBattleEnd;
            //         break;
            //     default:
            //         active = Trigger.Passive;
            //         break;
            // }
            string[] s = Enum.GetNames(typeof(Equipment.Trigger));

            Enum.TryParse(s[value],out Equipment.Trigger active);
        }
    }
}