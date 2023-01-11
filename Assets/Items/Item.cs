using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item: ScriptableObject
{
    public enum Grade{Common,Uncommon,Rare,Elite,Legendary,Phantasmal}
    public Grade grade = 0;
    public Sprite icon;
    public string name;
    public string description;
    public int rarity
    {
        get
        {
            switch(grade)
            {
                case Grade.Uncommon:
                    return 2;
                case Grade.Rare:
                    return 3;
                case Grade.Elite:
                    return 4;
                case Grade.Legendary:
                    return 5;
                case Grade.Phantasmal:
                    return 6;
                default:
                    return 1;
            }
        }

        set
        {
            switch(value)
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
    }
}