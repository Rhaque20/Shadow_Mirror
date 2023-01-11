using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageContact : DamageContact
{
    private EnemySkill es;
    private EnemyMobile em;
    [SerializeField]Collider2D player;

    public EnemySkill a_es
    {
        get{return es;}
        set{es = value;}
    }
    // Start is called before the first frame update
    void Start()
    {
        em = GetComponent<EnemyMobile>();
        stats = GetComponent<EnemyStats>();
    }

    public override void ReturnCollision()
    {
        Physics2D.IgnoreLayerCollision(6,7,false);
        if (player != null)
        {
            Physics2D.IgnoreCollision(player,self,false);
            player = null;
            hits = 0;
        }
    }

    public override void AttackOverlay(Collider2D col)
    {
        bool crit; 
        float bonus = 0f;

        if (col.gameObject.CompareTag("Player") && hasContact)
        {
            //Shadowpuppetry sp = col.gameObject.GetComponent<Shadowpuppetry>();
            Beatemupmove bm = col.gameObject.GetComponent<Beatemupmove>();
            PlayerInterrupt mainBody = col.gameObject.transform.GetChild(0).gameObject.GetComponent<PlayerInterrupt>();

            if (!bm.invinc)
            {

                Stats s = bm.parameters;

                crit = s.a_As.AccuracyCheck(0f);
                if (crit)
                {
                    crit = stats.DidCrit();
                    bonus = stats.DamageBonus(1.2f,crit,es.element);
                    mainBody.TriggerStagger(new Vector2(300f,0f),em.isRight);
                }
                else
                {
                    Debug.Log("MISS!");
                    bonus = stats.DamageBonus(1.2f * 0.7f,false,es.element);
                }
                
                //.stats.enemyInstance = enemy.gameObject;
                s.damagecalc(stats.GetFinalStat(1),es.power,crit,es.element);
                player = col.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(player,self.GetComponent<Collider2D>(),true);
                hits++;
            }
            else
                bm.PerfectTrigger();

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
