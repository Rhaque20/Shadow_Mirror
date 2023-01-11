using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHUD : MonoBehaviour
{
    public Image healthbar;
    public Stats stats;
    public Image stress;
    public TMP_Text healthtext,sptext;
    [SerializeField]Overdrive od;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StressChange(float value)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.fillAmount = stats.health/stats.maxHP;
        healthtext.text = stats.health.ToString();
        if (stress != null)
        {
            if (od.state == Overdrive.StressState.Neutral)
                stress.fillAmount = od.modeval/od.toODmax;
            else
                stress.fillAmount = od.modeval/od.toBreakmax;
        
            if (stress.fillAmount < 1.0f)
            {
                if (od.state == Overdrive.StressState.Neutral)
                    stress.color = new Color32(236,220,59,255);
                else
                    stress.color = new Color32(255,159,0,255);
            }
            if (stress.fillAmount >= 1f)
            {
                stress.color = new Color32(255,159,0,255);
            }

        }
        
        
    }
}
