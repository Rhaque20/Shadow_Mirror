using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Stats
{
    [SerializeField]private Overdrive od;
    [SerializeField]private EnemyCore ec;
    // 0 = HP 1 = ATK 2 = DEF 3 = Potency 4 = Resistance 5 = C. Rate 6 = C. DMG 7 = SP Gain
    [SerializeField]private float[] ODboost = new float[8];
    public EnemyVariables ev;

    // Start is called before the first frame update
    void Start()
    {
        sc = GetComponent<EnemyStatusChange>();
        health = maxHP;
    }



    private void StackableIncrease(int i)
    {

    }

    public override void Death()
    {
        Debug.Log("End my suffering");
        this.gameObject.SetActive(false);
    }

    public override float GetFinalStat(int val1)
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
                    s = sc.eff["ATK Up"];
                    mod += s.effect[s.chain];
                }

                if (od.state == Overdrive.StressState.Overdrive)
                {
                    mod += ODboost[1];
                }
                return attack * mod;
            // Defense
            case 2:
                if (sc.eff.ContainsKey("DEF Up"))
                {
                    s = sc.eff["DEF Up"];
                    mod += s.effect[s.chain];
                }

                if (od.state == Overdrive.StressState.Overdrive)
                {
                    mod += ODboost[2];
                }
                return defense * mod;
        }
        return 0;
    }

    public override float damagecalc(float atk, float dmgbonus, bool crit, int element)
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
            od.stoicHit(damage);
            
        }
        else
            damage = 0f;

        return damage;
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
        
        if (health <= 0f)
        {
            health = 0;
            od.TriggerKOAnim();
        }
        if (health >= maxHP)
            health = maxHP;
    }
}
