using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerEquipData
{
    public string nameofLoadOut;
    public PlayerArmorData[] armorLoadOut;
    public PlayerWeaponData weaponLoadOut;
    // Start is called before the first frame update
    public PlayerEquipData(CharacterEquips ce, string n)
    {
        armorLoadOut = new PlayerArmorData[6];
        for (int i = 0; i < 6; i++)
        {
            if (ce.armors[i] == null)
                continue;
            armorLoadOut[i] = new PlayerArmorData(ce.armors[i]);
        }
        if (ce.a_Weapon != null)
            weaponLoadOut = new PlayerWeaponData(ce.a_Weapon);
        else
        {
            weaponLoadOut = null;
        }
        nameofLoadOut = n;
    }
}
