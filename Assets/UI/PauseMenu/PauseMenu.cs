using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]List<GameObject> listofButtons = new List<GameObject>();
    public enum MenuState{Main,Items,Equipment,Skills};
    private MenuState ms = MenuState.Main;
    public int statenum = -1;
    [SerializeField]GameObject mainDisplay;
    [SerializeField]PlayerHUD phud;
    public bool menuActive = false;
    Vector2[] cachedVec2 = new Vector2[6];// 0 - 2 Position 3 - 5 Scale
    [SerializeField]Transform[] partySlots = new Transform[6];
    GameManager GM;
    [SerializeField]PlayerParty pp;
    [SerializeField]DisplayGear dg;
    
    GameObject equipmentMenu;

    public void InterpretInt(int i)
    {
        switch(i)
        {
            case 0:
                ms = MenuState.Items;
                break;
            case 1:
                ms = MenuState.Equipment;
                DisplayEquipment();
                break;
            case 2:
                ms = MenuState.Skills;
                break;
            default:
                ms = MenuState.Main;
                break;
        }
        
        switch(statenum)
        {
            case 1:
                equipmentMenu.SetActive(false);
            break;
        }

        statenum = i;
    }

    public void DisplayEquipment()
    {
        equipmentMenu.SetActive(true);
        RelocateDisplay(2);
        equipmentMenu.GetComponent<EquipmentMenu>().UpdateStats();
        dg.ChangeGearDisplay();
    }

    public void HideOtherButtons(int j)
    {
        if (j == statenum)
        {
            RevealButtons();
            return;
        }

        InterpretInt(j);
        listofButtons[j].GetComponent<Animator>().Play("Selected");
        for (int i = 0; i < listofButtons.Count; i++)
        {
            if (i == j)
                continue;
            
            listofButtons[i].SetActive(false);
        }
    }

    public void RevealButtons()
    {
        Animator anim;
        for (int i = 0; i < listofButtons.Count; i++)
        {
            listofButtons[i].SetActive(true);
            anim = listofButtons[i].GetComponent<Animator>();    
            anim.Play("Inactive");
        }
        InterpretInt(-1);
    }

    void Start()
    {
        GameObject g;
        pp = GameObject.Find("Party").GetComponent<PlayerParty>();
        dg = GameObject.Find("EquipmentBlock").GetComponent<DisplayGear>();

        mainDisplay = transform.GetChild(0).gameObject;
        phud.a_pm = GetComponent<PauseMenu>();

        int searchPos = 6;

        equipmentMenu = mainDisplay.transform.Find("EquipmentSubMenu").gameObject;
        equipmentMenu.GetComponent<EquipmentMenu>().a_phud = phud;
        equipmentMenu.GetComponent<EquipmentMenu>().a_pp = pp;
        equipmentMenu.SetActive(false);
        
        for (int i = 1 ; i < mainDisplay.transform.childCount; i++)
        {
            g = mainDisplay.transform.GetChild(i).gameObject;
            if (g.CompareTag("Button"))
            {
                listofButtons.Add(g);
            }
            else
            {
                partySlots[6 - searchPos] = g.transform;
                searchPos--;
                if (searchPos == 0)
                    break;
            }
                
        }

        phud = transform.parent.Find("Player_HUD").gameObject.GetComponent<PlayerHUD>();
        
        GM = phud.GM;
        mainDisplay.SetActive(false);
        for (int i = 0; i < phud.a_ps.Length; i++)
        {
            cachedVec2[i] = phud.a_ps[i].transform.position;
            cachedVec2[i+3] = phud.a_ps[i].transform.localScale;
        }
    }

    void RelocateDisplay(int mode)
    {
        PlayerStatus[] ps = phud.a_ps;
        switch(mode)
        {
            case 0:
            for (int i = 0; i < 3; i++)
            {
                ps[i].transform.position = cachedVec2[i];
                ps[i].transform.localScale = cachedVec2[i+3];
            }
            break;

            case 1:
            
            for (int i = 0; i < 3; i++)
            {
                //ps[i].transform.position = partySlots[i].position;
                //ps[i].transform.localScale = new Vector2(1.5f,1.5f);
            }
            break;

            case 2:

            for (int i = 0; i < 3; i++)
            {
                //ps[i].transform.position = partySlots[i+3].position;
                //ps[i].transform.localScale = new Vector2(1.5f,1.5f);
            }
            break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("x") && ms != MenuState.Main)
        {
            RevealButtons();
            RelocateDisplay(1);
        }

        if (Input.GetKeyDown("escape"))
        {
            if (statenum != -1)
            {
                HideOtherButtons(statenum);
                RelocateDisplay(1);
            }
            else
            {
                mainDisplay.SetActive(!menuActive);
                menuActive = !menuActive;
                phud.UIElementToggle(!menuActive);

                if (menuActive)
                {
                    GM.SetTimeScale(0.0f);
                    RelocateDisplay(1);
                }
                else
                {
                    GM.SetTimeScale(1.0f);
                    RelocateDisplay(0);
                }
            }
        }
    }
}
