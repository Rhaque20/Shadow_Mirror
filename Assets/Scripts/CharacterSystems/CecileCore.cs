using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CecileCore : CharacterCore
{
    private bool shadowAirborne = false;
    public GameObject slashPrefab,mirage;
    private GameObject backPointer;
    float mirageSkillMod = 0f;
    
    Skill activeSkill = null;
    // Start is called before the first frame update
    void Start()
    {
        c.SetLayerMask(enemyLayers);
        pi = GetComponent<PlayerInterrupt>();
        anim = GetComponent<Animator>();
        aoc = na.aoc;

        foreach(Skill s in skp.groundskills)
        {
            if (s.name.Contains("Dark Mirage"))
            {
                mirageSkillMod = s.power;
                break;
            }
        }
    }


    bool BackStab(GameObject enemy)
    {
        float direction = enemy.transform.position.x - transform.position.x;
        EnemyMobile sr = enemy.gameObject.GetComponent<EnemyMobile>();
        Debug.Log("BackSTAB!");
        if (direction > 0f && !sr.isRight)
            return true;
        else if (direction < 0f && sr.isRight)
            return true;
        
        return false;
    }

    void CreateShadow()
    {
        //Debug.Log("Creating shadow");
        Rigidbody2D shadowb;
        mirage.SetActive(true);
        mirage.transform.position = bm.curr_position();
        if (bm.a_flip)
            mirage.GetComponent<ProjectileScript>().Fire(new Vector2(-50f,0f));
        else
            mirage.GetComponent<ProjectileScript>().Fire(new Vector2(50f,0f));
        mirage.GetComponent<ProjectileScript>().modifier = mirageSkillMod;
        qc.Increment(5);
    }

    void DarkMirage(bool onground ,Skill s)
    {
        Debug.Log("Calling "+s.name);
        ProjectileScript p = mirage.GetComponent<ProjectileScript>();

        if (!p.ready)
        {
            // This code block will handle the teleportation and slashing.
            bm.transform.position = mirage.transform.GetChild(0).position;// Child 0 is shadow point
            List<AnimationClip> secondAnims = new List<AnimationClip>(2);
            secondAnims.Add(s.attackAnim[2]);
            secondAnims.Add(s.attackAnim[3]);
            if (shadowAirborne)// If shadow is in the air, displace player up to shadow position
            {
                // Child 1 is shadow position itself
                bm.SuspendFalling(mirage.transform.GetChild(1).localPosition,secondAnims[0].length + 0.5f);
            }
            na.PlayGenericSkill(secondAnims,false);
            return;
        }
        else
        {
            na.PlayGenericSkill(s.attackAnim,false);
            mirage.transform.GetChild(1).localPosition = transform.localPosition;// Place shadow position based on player height
            shadowAirborne = !bm.a_onground;// Keep track of how shadow was placed
        }

        activeSkill = s;
            
    }

    void PhantomSlash(Skill s, bool found)
    {
        if (!found)
        {
            pdc.InitalizeCharge(false,false,0f,s.power);
            List<AnimationClip> secondAnims = new List<AnimationClip>(2);
            secondAnims.Add(s.attackAnim[0]);
            secondAnims.Add(s.attackAnim[1]);
            na.PlayGenericSkill(s.attackAnim,false);
            Vector2 force = new Vector2(bm.transform.localScale.x * 1000f,0f);
            activeSkill = s;
            
            bm.Knockback(force);
        }
        else
        {
            backPointer = pdc.targets[0].transform.GetChild(0).Find("PositionPointers").Find("BackPointer").gameObject;
            aoc["Rec1"] = s.attackAnim[2];
            anim.Play("Rec");
            bm.a_flip = !bm.a_flip;
            bm.transform.position = backPointer.transform.position;
            DashRecover();
            pdc.ReturnCollision();
            backPointer = null;
            pdc.targets.Clear();
        }
    }

    void StormCutter(Skill s)
    {
        activeSkill = s;
        Vector2 force = new Vector2(bm.transform.localScale.x * 2000f,0f);
        bm.SuspendFalling(transform.localPosition,s.attackAnim[0].length + 0.5f);
        pdc.InitalizeCharge(true,false,0f,s.power);
        bm.Knockback(force);
        na.PlayGenericSkill(s.attackAnim,false);
        pdc.SetupAttack(s,stats.TotalATK);
    }

    

    
    protected override void ActivateSkill()
    {
        Skill s;
        bool matchElevation = false;
        
        s = skp.groundskills[skp.skillsel - 1];

        //Debug.Log("Activating skill "+s.name);
        if (s.elevation != EnumLibrary.Elevation.both)
            matchElevation = (bm.a_onground == (s.elevation == EnumLibrary.Elevation.grounded));
        else
            matchElevation = true;

        if (stats.a_curSP >= s.SPcost && matchElevation)
        {
            stats.SPChange(-s.SPcost);

            phud.SkillSelected(true);
            Immobilize();
            pi.armormode = PlayerInterrupt.ArmorType.SuperArmor;
            

            if (s.name.Contains("Dark Mirage"))
                DarkMirage(bm.a_onground,s);
            else if (s.name.Contains("Phantom Slash"))
                PhantomSlash(s,false);
            else if (s.name.Contains("Storm Cutter"))
                StormCutter(s);
            else
                na.PlayGenericSkill(s.attackAnim,false);
        }
        else
            skp.skillsel = 0;

    }

    // Update is called once per frame
    void Update()
    {
        //lowheight = sp.height - 1f;
        //highheight = sp.height + 1f;
        if (Input.GetKey("x") && !casting && !bm.a_immobile)
        {
            //Debug.Log("Selecting skill");
            SkillSelect();
        }

        if (pdc.hasContact && activeSkill.name.Contains("Phantom Slash"))
        {
            if (pdc.targets.Count > 0)
            {
                //Debug.Log("Activate Phantom Slash!");
                PhantomSlash(activeSkill,true);
            }
        }
    }
}
