using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayGear : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]GameObject[] armorSlots = new GameObject[6];
    PartyEquips pe;
    PlayerParty pp;
    [SerializeField]EquipmentSprites es;
    void Start()
    {
        Debug.Log("Preloaded gear");
        GameObject g = GameObject.Find("Party");
        pe = g.GetComponent<PartyEquips>();
        pp = g.GetComponent<PlayerParty>();

        for (int i = 0; i < 6; i++)
        {
            armorSlots[i] = transform.GetChild(i).gameObject;
        }

        //ChangeGearDisplay();
    }

    public Armor ReturnArmorData(int i)
    {
        Armor a = pe.party[pp.active].armors[i];
        if (string.Compare(a.name,"") != 0)
            return a;
        else
            return null;
    }
    

    public void ChangeGearDisplay()
    {
        CharacterEquips ce = pe.party[pp.active];
        Armor a;
        Image frame, icon;

        for (int i = 0; i < 6; i++)
        {
            a = ce.armors[i];
            frame = armorSlots[i].transform.GetChild(0).gameObject.GetComponent<Image>();
            icon = armorSlots[i].transform.GetChild(1).gameObject.GetComponent<Image>();
            if (string.Compare(a.name,"") != 0)
            {
                frame.sprite = es.rarityFrames[a.rarity - 1];
                icon.sprite =  es.GetArmorIcon(a.set.ToString(),a.slot.ToString());
            }
            else
            {
                frame.sprite = es.blankGear[0];
                icon.sprite = es.blankGear[i+1];
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
