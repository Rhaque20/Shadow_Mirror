using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PartyEquips : MonoBehaviour,ISaveLoadOut
{
    [SerializeField]private List<CharacterEquips> partyGear = new List<CharacterEquips>();
    PlayerParty pp;

    public List<CharacterEquips> party
    {
        get{return partyGear;}
    }

    public void LoadData(PlayerPartyLoadOut data)
    {
        bool newLoad = false;
        //Debug.Log("Data test "+data.equips.Count);
        if (data.equips.Count == 0)
        {
            Debug.Log("Nothing to load");
            return;
        }
        
        for (int i = 0; i < partyGear.Count; i++)
        {
            partyGear[i].FillLoadOut(data.equips[i]);
        }
    }
    public void SaveData(ref PlayerPartyLoadOut data)
    {
        Debug.Log("PartyGear count is "+partyGear.Count);
        bool newLoad = false;
        for (int i = 0; i < partyGear.Count; i++)
        {
            newLoad = (i == data.equips.Count);
            data.AddLoadOut(partyGear[i], i, newLoad);
        }
    }
}