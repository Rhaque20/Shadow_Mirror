using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAI : EnemyCore
{
    //List<Collider2D> hitPlayers = new List<Collider2D>();
    // Start is called before the first frame update
    //bool canuseBuff = false;
    void Start()
    {
        s = ea.s;
        od = GetComponent<Overdrive>();
        //em = GetComponent<EnemyMobile>();
    }

    void NeutralWolfHowl()
    {
        
    }

    void WolfHowl(List<Collider2D> colliders, StatusEffect se)
    {
        GameObject target;
        foreach(Collider2D c in colliders)
        {
            target = c.gameObject;
            if (target.CompareTag("enemy") || target.CompareTag("Player"))
            { 
                target.GetComponent<StatusChanges>().ApplyEffect(se);
            }
        }
        
    }

    void WolfHowl(List<Collider2D> colliders, StatusEffect se, StatusEffect se2)
    {
        GameObject target;
        foreach(Collider2D c in colliders)
        {
            target = c.gameObject;
            if (target.CompareTag("enemy"))
            {
                target.GetComponent<StatusChanges>().ApplyEffect(se);
            }
            else if (target.CompareTag("Player"))
            {
                target.GetComponent<StatusChanges>().ApplyEffect(se2);
            }
        }
        
    }

    public override void NonAttackProcess(List<Collider2D> targets)
    {
        StatusEffect se;

        if (targets.Count < 1)
            Debug.Log("Captured No one");

        if (activeSkill.name.Contains("Wolf Howl"))
        {
            
            se = activeSkill.chains[0];
            se.chain = activeSkill.chainLevel[0];
            switch(variant)
            {
                case EnumLibrary.Element.Physical:
                    WolfHowl(targets,se);
                    if (!ev.enemyStatus.eff.ContainsKey("ATK Up"))
                    {
                        Debug.Log("Applying Attack Up!");
                        ev.enemyStatus.ApplyEffect(se);
                    }
                break;
            }
        }
        else
            Debug.Log("Skill that is called instead is: "+activeSkill.name);
            
    }

    protected override void SkillSel()
    {
        float x = ea.DistanceX(),y = ea.DistanceY();
        int selection = 0;

        if (ea.RangeX(0f,6f,Mathf.Abs(x)) && ea.RangeY(0f,0.8f,Mathf.Abs(y)))
        {
            //selection = Random.Range(0,3);
            ea.Prepping(1);
            activeSkill = ea.Rigid(new Vector2(0.2f * Mathf.Sign(x),0f),0,0,true);
        }
        else if (!ev.enemyStatus.eff.ContainsKey("ATK Up") && canuseBuff)
        {
            ea.Prepping(1);
            activeSkill = ea.Rigid(new Vector2(0f,0f),1,0,false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ea.es.mode == EnemyScan.Mode.Searching && od.state != Overdrive.StressState.Break)
        {
            SkillSel();
        }

    }
}
