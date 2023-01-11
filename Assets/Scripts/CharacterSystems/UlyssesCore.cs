using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UlyssesCore : CharacterCore
{
    //[SerializeField]private DamageContact pdc;
    // Start is called before the first frame update
    void Start()
    {
        pi = GetComponent<PlayerInterrupt>();
    }

    void ActivateSkill()
    {
        Skill s;

        casting = true;
        phud.cansel = false;
        bm.ad = 0;
        
        if (bm.a_onground)
        {
            s = skp.groundskills[skp.skillsel - 1];
        }
        else
        {
            s = skp.airskills[skp.skillsel - 1];
        }

        Debug.Log("Activating skill "+s.name);

        stats.SPChange(-s.SPcost);

        na.PlayGenericSkill(s.attackAnim,false);

    }
    
    public override void Attack(int chain)
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
                power = 1.3f;
                triggershake = true;
            }
            else
                power = 0.9f;
                //triggershake = true;
                break;
                
            case 3:
                power = 1.7f;
                triggershake = true;
                pdc.hasContact = true;
                pdc.InitializeAttack(stats.TotalATK *0.6f + stats.TotalDEF * 0.7f,power);
                force = 30f;
                break;
        }

        StartCoroutine(ForceForward(5f));
        AttackSize(groundedSize);
        targets = AttackScan(power,1, 0,triggershake);
        if (targets.Count > 0)
            stats.SPonHit();
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("x") && !casting)
        {
            Debug.Log("Selecting skill");
            if (Input.GetKeyDown("up"))
                skp.skillsel = 1;
            if (Input.GetKeyDown("right"))
                skp.skillsel = 2;
            if (Input.GetKeyDown("down"))
                skp.skillsel = 3;
            if(Input.GetKeyDown("left"))
                skp.skillsel = 4;
            
            if (skp.skillsel != 0 && stats.a_curSP >= skp.groundskills[skp.skillsel - 1].SPcost)
            {
                //skillsel = na.skillsel;
                //Debug.Log("Skill sel is "+skillsel);
                ActivateSkill();
                
                //na.a_skilldelay = true;
            }
            else
                skp.skillsel = 0;
        }
    }
}
