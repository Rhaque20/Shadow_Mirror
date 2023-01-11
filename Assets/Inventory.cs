using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveEquipment
{
    [SerializeField]List<Armor> armors = new List<Armor>();
    // Start is called before the first frame update
    void Start()
    {
        //armors.Add(ArmorAttributes.GenerateArmor(45,"Bright Star",Armor.Sets.BrightStar));
        //armors.Add(ArmorAttributes.GenerateArmor(45,"Bright Star",Armor.Sets.BrightStar));
        //Debug.Log("Generated "+armors[0].name+" with rarity "+armors[0].grade);
        //armors[0].DisplaySubstat();

        //for(int i = 0; i < armors[0])
    }

    public void LoadData(ArmorInventoryData data)
    {
        if (data.listOfArmors.Count < 1)
            return;
        
        foreach(PlayerArmorData a in data.listOfArmors)
        {
            armors.Add(new Armor(a));
        }
        Debug.Log("Armor Trigger type int is "+armors[0].TriggerInt);
    }

    public void SaveData(ref ArmorInventoryData data)
    {
        int i = 0;
        //Armor b;
        if (data.listOfArmors.Count == 0)
        {
            foreach(Armor a in armors)
            {
                data.AddArmor(a);
            }
        }
        else
        {
            foreach(Armor a in armors)
            {
                //b = new Armor(data.listOfArmors[i]);
                if (string.Compare(a.guid,data.listOfArmors[i].guid) != 0)
                    data.AddArmor(a);
                
                i++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
