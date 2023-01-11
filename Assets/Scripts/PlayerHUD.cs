using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public Image hpbar,subbar;
    public Image spbar,subbar2;
    public PlayerParty pp;
    public Stats stats;
    public TMP_Text healthtext,sptext;
    public GameObject skillpallete;
    public float drainrate = 0.5f, pastfill = 0f;
    public SkillDisplay sd;
    public bool selecting = false;
    private bool canselect = true, inSkill = false;
    [SerializeField]private PlayerStatus []ps = new PlayerStatus[3];
    GameManager gm;
    CharacterCore cc;
    Beatemupmove bm;
    PauseMenu pm;

    [SerializeField]GameObject panel;

    public PlayerStatus[] a_ps
    {
        get{return ps;}
    }

    public GameManager GM
    {
        set{gm = value;}
        get{return gm;}
    }

    public PauseMenu a_pm
    {
        get{return pm;}
        set{pm = value;}
    }

    public bool cansel
    {
        set{canselect = value;}
        get{return canselect;}
    }

    public void UIElementToggle(bool toggle)
    {
        panel.SetActive(toggle);

    }

    // Start is called before the first frame update
    void Start()
    {
        hpbar.fillAmount = 1f;
        skillpallete.SetActive(false);
        gm = GameObject.Find("Karen").GetComponent<GameManager>();
        cc = pp.a_party[pp.active].GetComponent<CharacterCore>();
        bm = pp.a_party[pp.active].GetComponent<Beatemupmove>();
        panel = transform.Find("Panel").gameObject;
    }

    public void SkillSelected(bool skillChoice)
    {
        if (skillChoice)
        {
            inSkill = true;
        }
        
        Time.timeScale = gm.dts;
        skillpallete.SetActive(false);
        bm.a_canmove = !inSkill;
        selecting = false;
    }

    public void OutofSkill()
    {
        inSkill = false;
        bm.a_canmove = true;
    }

    public void Reinitialize(int i)
    {
        sd.pallete = pp.a_party[i].GetComponent<SkillPallete>();
        cc = pp.a_party[i].GetComponent<CharacterCore>();
        bm = pp.a_party[i].GetComponent<Beatemupmove>();
        stats = pp.a_party[i].GetComponent<Stats>();
    }


    // Update is called once per frame
    void Update()
    {
        
        
        if (!pm.menuActive)
        {
            if (canselect && Input.GetKey("x"))
            {
                    selecting = true;
                    Time.timeScale = 0.4f;
                    skillpallete.SetActive(true);
                    sd.DisplaySkill();
                    /**
                    if (Input.GetKey("up") || Input.GetKey("down") || Input.GetKey("left") || Input.GetKey("right") )
                        canselect = false;
                    **/
                    bm.a_canmove = false;
            }
            if (Input.GetKeyUp("x"))
            {
                SkillSelected(false);
            }
            else if (!canselect)
            {
                Time.timeScale = gm.dts;
                skillpallete.SetActive(false);
                selecting = false;
            }
        }
        /**
        if (Input.GetKeyUp("p"))
        {
            stats.health -= 50f;
        }
        if (Input.GetKeyUp("o"))
        {
            stats.health += 50f;
        }
        **/
    }
}
