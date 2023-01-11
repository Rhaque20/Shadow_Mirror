using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCore : MonoBehaviour
{
    public EnemyVariables ev;
    [SerializeField]protected EnemyAI ea;
    [SerializeField]public EnemyStats s;
    protected Overdrive od;
    protected EnemySkill activeSkill;
    [SerializeField]protected EnemyMobile em;
    [SerializeField]protected EnumLibrary.Element variant = EnumLibrary.Element.Physical;
    [SerializeField]protected bool canuseBuff = false;


    public EnemySkill skillsel
    {
        get{return activeSkill;}
        set{
            //Debug.Log("Setting skill to :"+value);
            activeSkill = value;
        }
    }
    // Start is called before the first frame update

    public virtual void AttackProcess(List<Collider2D> targets)
    {
        GameObject target, mainBody;
        Stats e;
        Beatemupmove bm;
        InterruptSystem interrupt;
        float bonus;
        bool crit;

        if (targets.Count == 0)
        {
            Debug.Log("No targets");
            return;
        }

        foreach(Collider2D enemy in targets)
        {
            if (enemy.gameObject.CompareTag("Player"))
            {
                target = enemy.gameObject;
                mainBody = target.transform.GetChild(0).gameObject;
                e = target.GetComponent<Stats>();
                bm = target.GetComponent<Beatemupmove>();
                interrupt = mainBody.GetComponent<InterruptSystem>();

                if (!bm.invinc)
                {
                    Debug.Log("enemy skill name is null: "+(activeSkill.name));
                    crit = s.DidCrit();
                    bonus = s.DamageBonus(activeSkill.power,crit,activeSkill.element);
                    e.damagecalc(s.TotalATK,bonus,false,activeSkill.element);
                    interrupt.TriggerStagger(new Vector2(100f,0f),em.isRight);
                }
                else
                    bm.PerfectTrigger();
            }


        }
        
    }

    public virtual void NonAttackProcess(List<Collider2D> targets)
    {

    }
    
    protected virtual void SkillSel()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void LoadUp()
    {
        s = ev.stats;
        em = ev.movement;
    }
}
