using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSideBar : MonoBehaviour
{
    public Image [] effectIcons = new Image[3];
    public Image [] skillIcon = new Image[2];// Index 0 is frame and index 1 is icon
    // 0 = Main Effect 1 = First Chain Effect 2 = Second Chain Effect 3 = First Chain text 4 = Second Chain text
    public TMP_Text [] text = new TMP_Text[5];
    public TMP_Text skillname;
    public TMP_Text cost;
    public string testname;
    public bool initialized = false;


    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        //Debug.Log(testname+" Has started initializing");
        // Gets the child of the skill object, "Icons"
        GameObject temp = this.transform.Find("Descriptions").Find("Icons").gameObject;
        int i;
        // Gathers the icons in the Icons Parent/Folder
        for (i = 0; i < 3; i++)
        {
            effectIcons[i] = temp.transform.GetChild(i).gameObject.GetComponent<Image>();// Debuff/Buff Icons
        }
        // Gets the child of the skill Object, "Text"
        temp = this.transform.Find("Descriptions").Find("Text").gameObject;
        // Gets the text in the "Text" folder from Main effect, First Chain and its effect and Second Chain and its effect
        for (i = 0; i < 5; i++)
        {
            text[i] = temp.transform.GetChild(i).gameObject.GetComponent<TMP_Text>();
        }
        // Gets the last two text which is the skill name and skill cost
        skillname = temp.transform.GetChild(5).gameObject.GetComponent<TMP_Text>();
        cost = temp.transform.GetChild(6).gameObject.GetComponent<TMP_Text>();

        initialized = true;

        temp = this.transform.Find("SkillFrame").gameObject;
        skillIcon[0] = temp.GetComponent<Image>();
        temp = this.transform.Find("SkillIcon").gameObject;
        skillIcon[1] = temp.GetComponent<Image>();
    }
    

    /**
    text[0] = Status Name (1)
    text[1] = Status Name (2)
    text[2] = Status Name (3)
    text[3] = Chain #
    text[4] = Chain #
    **/
}
