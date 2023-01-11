using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField]Stats stats;
    public Image hpbar,subbar;
    public Image spbar,subbar2;
    public TMP_Text healthtext,sptext,name;
    public float drainrate = 0.5f, pastfill = 0f;
    public GameObject Statuses;

    public void SetHealth(float health)
    {
        healthtext.text = stats.health.ToString()+"/"+stats.maxHP.ToString();
        hpbar.fillAmount = health;
        if (hpbar.fillAmount != subbar.fillAmount)
        {
            if (hpbar.fillAmount < subbar.fillAmount)
            {
                subbar.fillAmount = subbar.fillAmount - drainrate;
                if (pastfill == subbar.fillAmount)
                {
                    subbar.fillAmount = hpbar.fillAmount;
                }
                else
                    pastfill = subbar.fillAmount;
            }
            else if (hpbar.fillAmount >= subbar.fillAmount)
            {
                subbar.fillAmount = hpbar.fillAmount;
            }
        }
    }
    public void SetSPBar(float SP)
    {
        sptext.text = ((int)stats.a_curSP).ToString()+"/"+stats.a_maxSP.ToString();
        spbar.fillAmount = SP;
        if (spbar.fillAmount != subbar2.fillAmount)
        {
            if (spbar.fillAmount < subbar2.fillAmount)
            {
                subbar2.fillAmount -= drainrate;
                if (subbar2.fillAmount <= spbar.fillAmount)
                    subbar2.fillAmount = spbar.fillAmount;
            }
            else if (spbar.fillAmount > subbar2.fillAmount)
                subbar2.fillAmount = spbar.fillAmount;
        }
        //Debug.Log("SP ratio for "+name.text+" is: "+SP);
    }
    // Start is called before the first frame update
    void Start()
    {
        hpbar.fillAmount = 1f;
    }

    public void Loadup(Stats s, string insname)
    {
        stats = s;
        name.text = insname;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Updating at playerstatus");
        SetHealth(stats.health/stats.maxHP);
        SetSPBar(stats.a_curSP/stats.a_maxSP);
    }
}
