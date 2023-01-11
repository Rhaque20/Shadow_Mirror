using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerPartyLoadOut
{
    public List<PlayerEquipData> equips = new List<PlayerEquipData>();

    public PlayerPartyLoadOut()
    {
        
    }

    public void AddLoadOut(CharacterEquips ce, int i, bool newLoad)
    {
        if (newLoad)
            equips.Add(new PlayerEquipData(ce ,ce.gameObject.name));
        else
        {
            Debug.Log("Overriding with "+ce.armors[0].name);
            equips[i] = new PlayerEquipData(ce,ce.gameObject.name);
        }
    }


}