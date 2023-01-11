using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageContact : MonoBehaviour
{
    public Stats stats;
    public bool hasContact = false;
    [SerializeField]protected bool immediate = true, endOnCollision = true;
    public Collider2D self;
    public int hits;
    //protected Skill skill = null;
    protected float statVal;// Used for dual stat values;
    protected float modifier;// Used for non skill damage
    protected ContactFilter2D c = new ContactFilter2D();
    public LayerMask enemyLayers;
    protected List<Collider2D> hitEnemies = new List<Collider2D>();
    protected List<Collider2D> marked = new List<Collider2D>();

    
    /**
    public Skill a_s
    {
        get{return skill;}
        set{skill = value;}
    }
    **/

    public List<Collider2D> mark
    {
        get{return marked;}
    }

    public List<Collider2D> targets
    {
        get{return hitEnemies;}
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitializeContactType(bool val1, bool val2)
    {
        immediate = val1;
        endOnCollision = val2;
        if (!endOnCollision)
            Physics2D.IgnoreLayerCollision(6,7,true);
    }

    public void InitializeAttack(float sVal,float mod)
    {
        statVal = sVal;
        modifier = mod;
    }

    public virtual void ReturnCollision()
    {
        /**
        if (player != null)
        {
            Physics2D.IgnoreCollision(player,self.GetComponent<Collider2D>(),false);
            hits = 0;
        }
        **/
        Physics2D.IgnoreLayerCollision(6,7,false);
        marked.Clear();
        hasContact = false;
        //hitEnemies.Clear();
    }

    public virtual void AttackOverlay(Collider2D col)
    {
        bool crit; 
        float bonus = 0f;
        // Enemy Scan
        /**
        if (col.gameObject.CompareTag("enemy") && hasContact)// PlayerScan
        {
            //Shadowpuppetry sp = col.gameObject.GetComponent<Shadowpuppetry>();

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
            skill = null;
        }
        **/
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (hasContact)
            AttackOverlay(col.collider);
    }
    
    protected bool CheckMarked(Collider2D c)
    {
        foreach(Collider2D col in marked)
        {
            if (col == c)
                return true;
        }

        return false;
    }

    protected virtual void CollisionScan()
    {
        if (!hasContact)
            return;
        
        if(self.OverlapCollider(c,hitEnemies) > 0)
        {
            foreach(Collider2D col in hitEnemies)
            {
                if (!CheckMarked(col))
                {
                    marked.Add(col);
                    if (immediate)
                        AttackOverlay(col);
                }

            }

            if (endOnCollision)
            {
                hasContact = false;
                ReturnCollision();
            }
            //hasContact = false;
        }
    }


    // Update is called once per frame

    void Update()
    {
        /**
        if (hasContact)
        {
            if(player.OverlapCollider(c,hitEnemies) > 0)
            {
                foreach(Collider2D col in hitEnemies)
                {
                    if (col.gameObject.CompareTag("enemy") && !CheckMarked(col))
                    {
                        marked.Add(col);
                        AttackOverlay(col);
                    }
                }
                //hasContact = false;
            }
        }
        **/
        

    }
}
