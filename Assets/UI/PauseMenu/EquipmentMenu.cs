using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EquipmentMenu : MonoBehaviour
{
    PlayerHUD phud;
    PlayerParty pp;
    GearCard gc;
    DisplayGear dg;
    
    GameObject mainStatBlock, equipmentBlock;
    int active = 0;

    public PlayerHUD a_phud
    {
        get{return phud;}
        set{phud = value;}
    }

    public PlayerParty a_pp
    {
        get{return pp;}
        set{pp = value;}
    }



    // Start is called before the first frame update
    void Start()
    {
        mainStatBlock = transform.Find("MainStatBlock").gameObject;
        equipmentBlock = transform.Find("EquipmentBlock").gameObject;
        gc = equipmentBlock.transform.Find("EquipmentDisplay").GetComponent<GearCard>();
        dg = equipmentBlock.GetComponent<DisplayGear>();
        gc.gameObject.SetActive(false);
    }

    string StatName(int i)
    {
        PlayerStats s = pp.a_party[pp.active].GetComponent<PlayerStats>();
        switch(i)
        {
            case 0:
                return "Health: "+(Mathf.Floor(s.TotalHP)).ToString();
            case 1:
                return "Attack: "+(Mathf.Floor(s.TotalATK)).ToString();
            case 2:
                return "Defense: "+(Mathf.Floor(s.TotalDEF)).ToString();
            case 3:
                return "Potency: "+(s.TotalPotency * 100f).ToString()+"%";
            case 4:
                return "Resistance: "+(s.TotalResistance * 100f).ToString()+"%";
            case 5:
                return "Crit Rate: "+(s.TotalCritRate * 100f).ToString()+"%";
            case 6:
                return "Crit DMG: "+(s.TotalCritDMG * 100f).ToString()+"%";
            case 7:
                return "SP Gain: "+(s.TotalSPGain*100f).ToString()+"%";
        }

        return "Banana";
    }

    public void ActivateGearCard(int i)
    {
        Armor a;
        a = dg.ReturnArmorData(i);

        if (a != null)
        {
            ToggleCard(true);
            gc.UpdateCard(a);
        }
        else
        {
            ToggleCard(false);
        }
    }

    public void ToggleCard(bool toggle)
    {
        gc.gameObject.SetActive(toggle);
    }

    public void UpdateStats()
    {
        GameObject stat;
        for (int i = 0; i < mainStatBlock.transform.childCount - 1; i++)
        {
            stat = mainStatBlock.transform.GetChild(i).gameObject;
            stat.GetComponent<TMP_Text>().text = StatName(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // RaycastHit hit;
            // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // if (Physics.Raycast(ray, out hit))
            // {
            //     if (hit.collider != null)
            //     {
            //         //hit.collider.enabled = false;
            //         Debug.Log("Hit "+hit.collider.gameObject.name);
            //     }
                
            // }

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log("Clicked out of Bounds");
                ToggleCard(false);
            }
        }
    }
}
