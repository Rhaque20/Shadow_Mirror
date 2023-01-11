using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stats : MonoBehaviour
{
    /**
       0     1     2     3     4     5     6     7     8
    +-----+-----+-----+-----+-----+-----+-----+-----+-----+
    |  HP | ATK | DEF | POT | RES | CRI | CDMG|SPGAI| SP  |
    +-----+-----+-----+-----+-----+-----+-----+-----+-----+
    **/
    // Stat slot 8 will be OD Multiplier
    public float health,maxHP,attack,defense,critrate = 0.05f,critdmg = 0.5f,
    potency = 0f, resistance = 0f, spgain = 1.0f;
    public int level;
    [SerializeField]protected float curSP = 300f,maxSP = 300f;
    [HideInInspector]public float damage;
    [HideInInspector]public float mercy = 0f;
    public float low = 1f,high = 1f;
    [SerializeField]protected AdvancedStats As;
    public StatusChanges sc;
    
    //public bool initialized = false;


    // DamageText Variables
    public GameObject damageTextPrefab, enemyInstance;
    public Vector3 positioning;
    // Start is called before the first frame update

    

    public AdvancedStats a_As
    {
        get {return As; }
    }

    public virtual float TotalHP
    {
        get{return GetFinalStat(0);}
    }

    public virtual float TotalATK
    {
        get {
            return GetFinalStat(1);
        }
    }

    public virtual float TotalDEF
    {
        get {
            return GetFinalStat(2);
        }
    }

    public virtual float TotalCritRate
    {
        get{
            return GetFinalStat(5);
        }
    }
    public virtual float TotalCritDMG
    {
        get{
            return GetFinalStat(6);
        }
    }

    public virtual float a_curSP
    {
        get{return curSP;}
        set{curSP = value;}
    }

    public virtual float a_maxSP
    {
        get{return maxSP;}
        set{maxSP = value;}
    }

    public virtual void Death()
    {
        
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

    void OnTriggerEnter2D(Collider2D collision)
    {
    }

    public bool DidCrit()
    {
        float rng = Random.Range(0f,1f);

        return (rng <= critrate);
    }

    public float DamageBonus(float mod, bool critical, int element)
    {
        // Advanced Stats EleDMG will do 1 + Elemental Dmg specified by number
        float totalbonus = mod * Random.Range(low,high) * As.EleDMG(element);
        if (critical)
            return totalbonus * (1+ critdmg);
        else
            return totalbonus;
    }

    public float DamageResist(int element)
    {
        // Eleres will return 1 - specified res and Reduction will do the same
        float totalRes = As.EleRES(element) * As.Reduction;

        return totalRes;
    }

    public virtual float GetFinalStat(int val1)
    {
        float mod = 1f;
        StatusEffect s;
        switch(val1)
        {
            case 0:
                return maxHP;
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
                return attack * mod;
            // Defense
            case 2:
                if (sc.eff.ContainsKey("DEF Up"))
                {
                    s = sc.eff["DEF Up"];
                    mod += s.effect[s.chain];
                }
                return defense * mod;
            case 5:
                return critrate;
        }

        return 0;
    }

    protected void DamagePrint(bool crit,int type)
    {
        GameObject DamageText;
        positioning = enemyInstance.transform.position;
        positioning = new Vector3(positioning.x + 2f + Random.Range(-1f,1f),positioning.y + Random.Range(-1f,1f),positioning.z);

        DamageText = Instantiate(damageTextPrefab, positioning,Quaternion.identity);
        if (crit)
            DamageText.transform.GetChild(0).GetComponent<Animator>().Play("Normal_Crit");
        else
            DamageText.transform.GetChild(0).GetComponent<Animator>().Play("Normal_DMG");
        TextMeshPro dmgtext = DamageText.transform.GetChild(0).GetComponent<TextMeshPro>();
        if (type == 0)
            dmgtext.SetText(damage.ToString());
        else
            dmgtext.SetText("<sprite="+(type-1).ToString()+">"+damage.ToString());
    }

    

    public virtual float damagecalc(float atk, float dmgbonus, bool crit, int element)
    {
        if (mercy == 0)
        {
            float DEF = GetFinalStat(2);
            damage = atk/(TotalDEF/300+1);
            //Debug.Log("damage is" +damage);
            damage *= dmgbonus * DamageResist(element);
            damage = Mathf.Round(damage);
            health -= damage;

            DamagePrint(crit, element);
            mercy = 0.01f;
            
        }
        else
            damage = 0f;

        return damage;
    }

    public void SPChange(float change)
    {
        //Debug.Log("Changed by "+change);
        curSP += change;

        if (curSP < 0)
            curSP = 0f;
        if (curSP > maxSP)
        {
            curSP = maxSP;
        }
    }

    public virtual void SPonHit()
    {
        if (curSP < maxSP)
            curSP += 5 * spgain;
        if (curSP > maxSP)
        {
            curSP = maxSP;
        }
    }

    void FixedUpdate()
    {
        /**
        if (Input.GetKeyDown("p") && id == 1)
        {
            health -= 30;
            Debug.Log("Cecile took 30 Damage!");
        }

        if (Input.GetKeyDown("o") && id == 1)
        {
            health += 30;
            Debug.Log("Cecile restored 30 HP!");
        }
        **/
    }

    // Update is called once per frame
    void Update()
    {
        if (mercy != 0)
        {
            mercy -= Time.deltaTime;
            if (mercy <= 0)
            {
                mercy = 0;
            }
        }
        
        if (health <= 0)
            health = 0;
        if (health >= maxHP)
            health = maxHP;

    }
}
