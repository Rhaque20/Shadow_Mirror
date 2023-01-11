using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ArmorInventoryData
{
    public List<PlayerArmorData> armors = new List<PlayerArmorData>();

    public List<PlayerArmorData> listOfArmors
    {
        get{return armors;}
    }

    public ArmorInventoryData()
    {
        
    }

    public void AddArmor(Armor a)
    {
        //Debug.Log("Added "+a.name);
    
        armors.Add(new PlayerArmorData(a));
    }
}