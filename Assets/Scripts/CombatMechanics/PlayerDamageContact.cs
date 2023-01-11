using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageContact : DamageContact
{
    Beatemupmove bm;
    Skill skill;
    private QTEChain qc;

    public QTEChain qte
    {
        set{qc = value;}
        get{return qc;}
    }
    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Collider2D>();
        stats = GetComponent<PlayerStats>();
    }

    public override void ReturnCollision()
    {
        /**
        if (player != null)
        {
            Physics2D.IgnoreCollision(player,self.GetComponent<Collider2D>(),false);
            hits = 0;
        }
        **/
        
        if (marked.Count > 0)
        {
            if (skill != null)
                qc.Increment(skill.element - 1);
            
            if (Physics2D.GetIgnoreLayerCollision(6,7))
            {
                float temp1 = 0f, temp2 = 0f, closest;
                Vector2 backposition = marked[0].transform.GetChild(0).Find("PositionPointers").Find("BackPointer").position;
                Vector2 frontposition = marked[0].transform.GetChild(0).Find("PositionPointers").Find("FrontPointer").position;
                Vector2 finalposition;
                temp1 = Vector2.Distance(transform.position,frontposition);
                temp2 = Vector2.Distance(transform.position, backposition);
                
                
                if (temp1 > temp2)
                {
                    finalposition = backposition;
                    closest = temp2;
                }
                else
                {
                    finalposition = frontposition;
                    closest = temp1;
                }
                
                foreach (Collider2D c in marked)
                {
                    frontposition = c.transform.GetChild(0).Find("PositionPointers").Find("FrontPointer").position;
                    backposition = c.transform.GetChild(0).Find("PositionPointers").Find("BackPointer").position;

                    temp1 = Vector2.Distance(transform.position, frontposition);
                    temp2 = Vector2.Distance(transform.position, backposition);

                    if (temp1 > temp2)
                    {
                        temp1 = temp2;
                        frontposition = backposition;
                    }

                    if (temp1 < closest)
                    {
                        closest = temp1;
                        
                        finalposition = frontposition;
                    }

                }

                transform.position = finalposition;
            }
        }
        Physics2D.IgnoreLayerCollision(6,7,false);
        marked.Clear();
        hasContact = false;
        skill = null;
        //hitEnemies.Clear();
    }

    public void SetupAttack(Skill s, float preloadedval)
    {
        skill = s;
        InitializeAttack(preloadedval, s.power);
    }
    
    public void InitalizeCharge(bool val1, bool val2, float sval, float smod)
    {
        InitializeContactType(val1,val2);// Immediate and endOnCollision
        InitializeAttack(sval,smod);
        hasContact = true;
    }

    public override void AttackOverlay(Collider2D col)
    {
        bool crit; 
        float bonus = 0f;

        if (!col.gameObject.CompareTag("enemy"))
            return;

        Stats s = col.gameObject.GetComponent<Stats>();

        crit = s.a_As.AccuracyCheck(0f);
        if (crit)
        {
            crit = stats.DidCrit();

            if (skill != null)
                bonus = stats.DamageBonus(modifier,crit,skill.element);
            else
                bonus = stats.DamageBonus(modifier,crit,0);
        }
        else
        {
            Debug.Log("MISS!");
            if (skill != null)
                bonus = stats.DamageBonus(modifier * 0.7f,crit,skill.element);
            else
                bonus = stats.DamageBonus(modifier * 0.7f,crit,0);
        }
        
        //.stats.enemyInstance = enemy.gameObject;
        if (skill != null)
            s.damagecalc(statVal,bonus,crit,skill.element);
        else
            s.damagecalc(statVal,bonus,crit,0);
        
        //player = col.GetComponent<Collider2D>();
        //Physics2D.IgnoreCollision(player,self.GetComponent<Collider2D>(),true);
        //hits++;
        //skill = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasContact)// PlayerScan
        {
            CollisionScan();
        }
    }
}
