using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCore : MonoBehaviour
{
    
    public PlayerStats stats;// Overridable
    public SkillPallete skp;// Overridable
    [SerializeField]protected Beatemupmove bm;// Overridable
    public Transform attackPoint,groundedPoint,player;// Overridable
    public NAAnims na;// Overridable
    public QTEChain qc;// Need programmer input
    [SerializeField]CameraMove cm;// Need programmer Input
    public PlayerHUD phud;// Need programmer input
    protected int dir = 1;
    public LayerMask enemyLayers;
    protected PlayerInterrupt pi;

    protected Animator anim;
    public AnimatorOverrideController aoc;
    [SerializeField]protected PlayerDamageContact pdc;

    protected int part = 1;
    public float height = 0f,lowheight = 0f,highheight = 0f;
    protected float power,force;
    protected ContactFilter2D c = new ContactFilter2D();
    protected bool casting = false;

    //[SerializeField]ChangeSlash[] cs = new ChangeSlash[2];

    [SerializeField]protected PlayerParty pp;

    public PlayerParty a_pp
    {
        set{pp = value;}
        get{return pp;}
    }

    public void SkillRecover()
    {
        casting = false;
        phud.OutofSkill();
        skp.skillsel = 0;
        bm.cd = false;
        na.a_na.a_canNormal = true;
        bm.ad = 1;
        phud.cansel = true;
        pi.armormode = PlayerInterrupt.ArmorType.Neutral;
        
        skp.QTEvanish();
    }

    /**
    void SlashColor(int element)
    {
        switch(element)
        {
            case 1:
                cs[0].PrimaryEleSlash();
                cs[1].PrimaryEleSlash();
                break;
            case 2:
                cs[0].SecondaryEleSlash();
                cs[1].SecondaryEleSlash();
                break;
            default:
                cs[0].PhysicalSlash();
                cs[1].PhysicalSlash();
                break;
        }
    }
    **/

    protected void DashRecover()
    {
        pdc.ReturnCollision();
    }

    protected void AttackSize(Vector2 size)
    {
        groundedPoint.transform.localScale = size;
    }

    public void Immobilize()
    {
        bm.a_canmove = false;
        bm.cd = false;
        bm.ad = 0;
        na.a_na.a_canNormal = false;
        casting = true;
        phud.cansel = false;
    }

    // Start is called before the first frame update
    void ActivateSkillChains(List<Collider2D> hitEnemies, Skill s)
    {
        StatusEffect status;
        StatusEffect regular = s.ApplyChain(0);

        if (qc.chain == 0)
        {
            status = null;

        }
        else
            status = s.ApplyChain(qc.chain);

        if (s.t[0] == EnumLibrary.Target.party)
            pp.ApplyEffecttoParty(regular, 0.3f);

         if (regular == null && status == null)
         {
            Debug.Log("Couldn't find any debuffs/buffs");
            return;
         }

        foreach (Collider2D enemy in hitEnemies)
        {
            StatusChanges sc = enemy.gameObject.GetComponent<StatusChanges>();
            if (status != null && !status.isBuff())
                sc.ApplyCheck(status,stats.potency);
            if (regular != null && !regular.isBuff())
                sc.ApplyCheck(regular, stats.potency);
        }

    }

    public void SkillSelect()
    {
        if (Input.GetKeyDown("up"))
            skp.skillsel = 1;
        if (Input.GetKeyDown("right"))
            skp.skillsel = 2;
        if (Input.GetKeyDown("down"))
            skp.skillsel = 3;
        if(Input.GetKeyDown("left"))
            skp.skillsel = 4;
        
        if (skp.skillsel != 0 && skp.groundskills[skp.skillsel - 1] != null)
        {
            //skillsel = na.skillsel;
            //Debug.Log("Skill sel is "+skillsel);
            ActivateSkill();
            
            //na.a_skilldelay = true;
        }
        else
            skp.skillsel = 0;
    }

    //Overridable
    public virtual void Attack(int chain)
    {
        bool triggershake = false;
        List<Collider2D> targets;
        Vector2 groundedSize = new Vector2(1f,1f);
        /**
        sp.shadowb.WakeUp();
            if (sp.onground)
                sp.shadowb.gravityScale = 0f;
        **/

        //PosofAttackZone();

        switch(chain)
        {
            case 1:
                power = 0.9f;
                triggershake = true;
                break;
            case 2:
            if (bm.a_onground)
            {
                if (part == 1)
                {
                    power = 0.75f;
                    part++;
                }
                else
                {
                    power = 1.2f;
                    part = 1;
                }
            }
            else
                power = 0.9f;
                //triggershake = true;
                break;
                
            case 3:
                power = 1.1f;
                triggershake = true;
                break;
            case 4:
                if (part == 1)
                {
                    power = 1.1f;
                    part++;
                }
                else
                {
                    power = 1.5f;
                    part = 1;
                }
                break;
            case 5:
                power = 1.1f;
                break;
            case 6:
                power = 1.2f;
                triggershake = true;
                 break;
        }

        StartCoroutine(ForceForward(200f));
        AttackSize(groundedSize);
        targets = AttackScan(power,1, 0,triggershake);
        if (targets.Count > 0)
            stats.SPonHit();
    }

    protected IEnumerator ForceForward(float force)
    {
        float duration = 0f;

        Rigidbody2D party = bm.a_party_rb, player = bm.a_rb;
        Vector2 dir = bm.transform.localScale;
        //party.velocity = new Vector2(force * dir.x, party.velocity.y);
        party.AddForce(new Vector2(force * dir.x,0f));
        player.isKinematic = true;
        party.drag = 0f;


        while (duration < 0.1f)
        {
            duration += Time.deltaTime;
            yield return null;
        }

        player.isKinematic = false;
        party.drag = 1000f;
    }

    protected virtual void ActivateSkill()
    {
        
    }

    protected void SkillAttackScan(int knockback)
    {
        Debug.Log("Skillsel is "+skp.skillsel);
        List<Collider2D> targets;
        Skill s = skp.groundskills[skp.skillsel - 1];
        //skillsel = 0;
        //na.skillsel = 0;
        Vector2 range;
        
        //range = new Vector2(6.07f,1.25f);
        //if (skp.groundskills[skillsel].name.Contains("Dark Thunder Blade"))
        targets = AttackScan(s.power,knockback,s.element,false);
        if (targets.Count > 0)
        {
            ActivateSkillChains(targets,s);
            qc.Increment(s.element - 1);
        }
        else
        {
            Debug.Log("No enemies found");
        }
        
    }

    protected virtual List<Collider2D> AttackScan(float power, int kbtype, int element, bool allowshake)
    {
        //Debug.Log("Step 2");
        List<Collider2D> hitEnemies = new List<Collider2D>();
        Forces jedi;
        
        GameObject target;
        bool shaketime = false;
        BoxCollider2D bc;

        if (bm.a_onground)
            bc = attackPoint.GetComponent<BoxCollider2D>();
        else
        {
            bc = groundedPoint.GetComponent<BoxCollider2D>();
        }
        //hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position,new Vector2(size.x,size.y),0f,enemyLayers);
        
        
        if (bc.OverlapCollider(c,hitEnemies) > 0)
        {
            foreach(Collider2D enemy in hitEnemies)
            {
                
                if (enemy.gameObject.CompareTag("enemy"))
                {
                    
                    shaketime = true;
                    jedi = enemy.gameObject.GetComponent<Forces>();
                    //Debug.Log("Cecile hits: "+enemy.name);


                    bool crit;
                    float bonus, damage;
                    //SlashEffect(jedi.puppet.transform.position);
                    if (kbtype == 1)
                        force = 5f;
                    else
                        force = 500f;
                    
                    if (jedi.stats.mercy <= 0f)
                    {
                        jedi.Stagger(bm.a_flip,kbtype,force);
                        crit = stats.DidCrit();
                        /**
                        if (BackStab(enemy.gameObject))
                        {
                            power += 0.25f;
                            Debug.Log("BACKSTAB!");
                        }
                        **/
                        
                        jedi.stats.enemyInstance = enemy.gameObject;
                        bonus = stats.DamageBonus(power,crit,element);
                        damage = jedi.stats.damagecalc(stats.TotalATK,bonus,crit,element);
                        jedi.od.StressFill(damage);
                        

                    }
                    
                }
            }
        }

        if (shaketime && allowshake)
            cm.StartShake(0.1f,0.1f,false);
        
        return hitEnemies;
    }
    
}
