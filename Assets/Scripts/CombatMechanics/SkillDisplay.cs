using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDisplay : MonoBehaviour
{
    // Disable color: #B2A6A6 #919191
    public GameObject[] skillobjects = new GameObject[4];
    public SkillPallete pallete;
    public Shadowpuppetry sp;
    public Stats curChar;
    public QTEChain qc;
    public SkillSideBar [] display = new SkillSideBar[4];
    public Color32 disable,fade;
    public PlayerParty pp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Color32 EnableTextColor(float SP)
    {
        if (curChar.a_curSP < SP)
        {
            return disable;
        }

        return Color.white;
    }

    public Color32 EnableTextColor(float SP, int chain)
    {
        if (chain != qc.chain || curChar.a_curSP < SP)
        {
            return disable;
        }

        return Color.white;
    }

    public Color32 EnableIconColor(float SP)
    {
        if (curChar.a_curSP < SP)
        {
            return fade;
        }

        return Color.white;
    }

    public Color32 EnableIconColor(float SP, int chain)
    {
        if (chain != qc.chain || curChar.a_curSP < SP)
        {
            return fade;
        }

        return Color.white;
    }

    public void DisableDisplay(SkillSideBar s)
    {
        int i;
        for (i = 0; i < 3; i++)
        {
            s.effectIcons[i].enabled = false;
        }

        for (i = 0; i < 5; i++)
        {
            s.text[i].enabled = false;
        }
        s.skillname.enabled = false;
        s.cost.enabled = false;
        s.skillIcon[1].enabled = false;
    }

    public void InitializeRow(int i,int x, int y, Skill curSkill)
    {
        display[i].effectIcons[x].enabled = true;// Effect Icon 2
        display[i].text[x].enabled = true;// Chain Effect
        display[i].text[y].enabled = true;// Chain Requirement

        display[i].text[x].color = EnableTextColor(curSkill.SPcost,curSkill.skillchain[x]);// Setting Chain Effect
        display[i].text[y].color = EnableTextColor(curSkill.SPcost,curSkill.skillchain[x]);// Setting Chain #
        display[i].effectIcons[x].color = EnableIconColor(curSkill.SPcost,curSkill.skillchain[x]);//Activate Icon Color

        display[i].text[x].text = curSkill.chains[x].name;
        display[i].text[y].text = "Chain " + curSkill.skillchain[x].ToString();
        display[i].effectIcons[x].sprite = curSkill.chains[x].Icon;
    }

    public void DisplaySkill()
    {
        int j;
        curChar = pp.a_party[pp.active].GetComponent<PlayerStats>();
        for (int i = 0; i < 4; i++)
        {
            
            Skill curSkill = pallete.groundskills[i];// Gets current skill

            if (!(display[i].initialized))// If it wasn't intialized, reinitialize
                display[i].Initialize();
            if (curSkill == null)// If skill is null, move on
            {
                DisableDisplay(display[i]);
                continue;
            }

            
            display[i].skillname.enabled = true;
            display[i].cost.enabled = true;
            display[i].skillIcon[1].enabled = true;
            display[i].skillIcon[1].sprite = curSkill.Icon; // Skill Icon
            display[i].skillname.text = curSkill.name; // Name of Skill

            display[i].skillname.color = EnableTextColor(curSkill.SPcost);// Set up mana amount
            display[i].skillIcon[1].color = EnableIconColor(curSkill.SPcost);// Set up if skill is ready

            // Default Effect
            if (curSkill.skillchain[0] != -1)// If default isn't non-existent
            {
                display[i].effectIcons[0].enabled = true;
                display[i].text[0].enabled = true;

                display[i].text[0].text = curSkill.chains[0].name;
                display[i].effectIcons[0].sprite = curSkill.chains[0].Icon;
                display[i].text[0].color = EnableTextColor(curSkill.SPcost);
                display[i].effectIcons[0].color = EnableIconColor(curSkill.SPcost);
                

            }
            else
            {
                display[i].effectIcons[0].enabled = false;
                display[i].text[0].enabled = false;
            }
            // First Chain Effect
            if (curSkill.skillchain[1] != -1)
            {
                /**
                display[i].effectIcons[1].enabled = true;// Effect Icon 2
                display[i].text[1].enabled = true;// Chain Effect
                display[i].text[3].enabled = true;// Chain Requirement

                display[i].text[3].color = EnableTextColor(curSkill.SPcost,curSkill.skillchain[1]);// Setting Chain #
                display[i].text[1].color = EnableTextColor(curSkill.SPcost,curSkill.skillchain[1]);// Setting Chain Effect
                display[i].effectIcons[1].color = EnableIconColor(curSkill.SPcost,curSkill.skillchain[1]);//Activate Icon Color

                display[i].text[3].text = "Chain " + curSkill.skillchain[1].ToString();
                display[i].text[1].text = curSkill.chains[1].name;
                display[i].effectIcons[1].sprite = curSkill.chains[1].Icon;
                **/
                InitializeRow(i,1,3,curSkill);

                // Second Chain Effect
                if (curSkill.skillchain[2] != -1)
                {
                    InitializeRow(i,2,4,curSkill);
                }
                else
                {
                    display[i].effectIcons[2].enabled = false;
                    display[i].text[2].enabled = false;
                    display[i].text[4].enabled = false;
                }
            }
            else
            {
                display[i].effectIcons[1].enabled = false;
                display[i].text[1].enabled = false;
                display[i].text[3].enabled = false;

                display[i].effectIcons[2].enabled = false;
                display[i].text[2].enabled = false;
                display[i].text[4].enabled = false;
            }

            display[i].cost.text = "SP Cost: "+curSkill.SPcost.ToString();
            display[i].cost.color = EnableTextColor(curSkill.SPcost);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
