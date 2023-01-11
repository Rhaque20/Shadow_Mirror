using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : Stats
{
    /**
       0     1     2     3     4     5     6     7     8
    +-----+-----+-----+-----+-----+-----+-----+-----+-----+
    |  HP | ATK | DEF | POT | RES | CRI | CDMG|SPGAI| SP  |
    +-----+-----+-----+-----+-----+-----+-----+-----+-----+
    **/

    [SerializeField]CharacterEquips ce;

    public CharacterEquips equipment
    {
        get{return ce;}
        set{ce = value;}
    }

    public override float TotalHP
    {
        get{return GetFinalStat(0);}
    }

    public override float TotalATK
    {
        get {
            return GetFinalStat(1);
        }
    }

    public override float TotalDEF
    {
        get {
            return GetFinalStat(2);
        }
    }

    public float TotalPotency
    {
        get{
            return GetFinalStat(3);
        }
    }

    public float TotalResistance
    {
        get{
            return GetFinalStat(4);
        }
    }

    public override float TotalCritRate
    {
        get{
            return GetFinalStat(5);
        }
    }
    public override float TotalCritDMG
    {
        get{
            return GetFinalStat(6);
        }
    }

    public float TotalSPGain
    {
        get{
            return GetFinalStat(7);
        }
    }

    public override float GetFinalStat(int val1)
    {
        float mod = 0f, statval = 0f;
        StatusEffect s;
        switch(val1)
        {
            case 0:
                statval = maxHP;
                break;
            // Attack
            case 1:
                if (sc.eff.ContainsKey("ATK Down"))
                {
                    s = sc.eff["ATK Down"];
                    mod -= s.effect[s.chain];
                }
                if (sc.eff.ContainsKey("ATK Up"))
                {
                    Debug.Log("Attack up present");
                    s = sc.eff["ATK Up"];
                    mod += s.effect[s.chain];
                }
                statval = attack;
                break;
            // Defense
            case 2:
                if (sc.eff.ContainsKey("DEF Up"))
                {
                    s = sc.eff["DEF Up"];
                    mod += s.effect[s.chain];
                }
                statval = defense;
                break;
            case 3:
                statval = potency;
                break;
            case 4:
                statval = resistance;
                break;
            case 5:
                statval = critrate;
                break;
            case 6:
                statval = critdmg;
                break;
            case 7:
                statval = spgain;
                break;
        }
        if (val1 < 3)
            return statval *(1 + ce.totalStats[val1]) * (1 + mod);
        else
            return statval + ce.totalStats[val1] + mod;
    }

    public override void SPonHit()
    {
        if (curSP < maxSP)
            curSP += 5 * TotalSPGain;
        if (curSP > maxSP)
        {
            curSP = maxSP;
        }
    }

    public void Start()
    {
        //height = sr.bounds.size.y;
        /**
            switch(id)
            {
                // Cecile;
                case 1:
                health = 145f + ((1073f/99f)*level);
                health = Mathf.Round(health);
                attack = 45f + ((480f/99f)*(level));
                defense = 36f + ((322f/99f)*level);
                curSP = 300f;
                break;

                case 2:
                health = 220f + ((1550f/99f)*level);
                health = Mathf.Round(health);
                attack = 49f + ((300f/99f)*(level));
                defense = 80f + ((450f/99f)*level);
                
                curSP = 300f;
                break;

            }
            maxHP = health;
            maxSP = curSP;
            critrate = 5f;
            critdmg = 0.5f;
            **/
            As = GetComponent<AdvancedStats>();
            
    }
}