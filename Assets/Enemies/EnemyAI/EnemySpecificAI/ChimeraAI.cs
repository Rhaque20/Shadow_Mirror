using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraAI : EnemyCore
{
    
    [SerializeField]GameObject fireballSet;
    Vector2 firebasePos;
    // Will be called whenever its off attack cooldown
    protected override void SkillSel()
    {
            // Calculate the distance between the enemy and player and determine what attacks to use.
            float x = ea.DistanceX(),y = ea.DistanceY();
            int selection = 0;
            
            // This is for frontal attacks
            if (ea.RangeX(4f,10f,Mathf.Abs(x)) && ea.RangeY(0f,0.8f,Mathf.Abs(y)))
            {
                selection = Random.Range(0,3);
                ea.Prepping(1);
                if (selection < 2)// 0 = Claw and 1 = Flame Wave
                {
                    activeSkill = ea.Rigid(new Vector2(Mathf.Sign(x)*3.5f,0f),0,0,false);// Use either claw smash or flame wave
                }
                else
                {
                    activeSkill = ea.FreeAim(selection,true);// Use Glacial Charge

                }
            }
            else if(ea.RangeX(0f, 5f,Mathf.Abs(x)) && ea.RangeY(0.8f, 4f,Mathf.Abs(y)))// If the player tries to flank them from the top/bottom
            {
                selection = Random.Range(2,4);
                ea.Prepping(1);
                if (selection == 2)
                {
                    activeSkill = ea.FreeAim(selection,true);// Use glacial charge
                }
                else if (selection < 5)
                {
                    //Debug.Log("Y distance is "+y);
                    
                    activeSkill = ea.Rigid(new Vector2(0f,Mathf.Sign(y)* 2.0f),3,2,false);// Snake Lightning
                    //selection = 3;
                }
                else
                {
                    activeSkill = ea.Tracker(selection);
                }
                
            }
            else if(ea.RangeX(4f, 15f,Mathf.Abs(x)) && ea.RangeY(0f, 3f,Mathf.Abs(y)))// Really far away
            {
                if(od.state == Overdrive.StressState.Overdrive)
                    selection = Random.Range(1,6);
                else
                    selection = Random.Range(1,3);
                
                ea.Prepping(1);
                if (selection == 2)
                {
                    activeSkill = ea.FreeAim(selection,true);// Does glacial charge
                }
                else if (selection == 1)
                {
                    fireballSet.transform.position = transform.position;
                    //ea.es.mode = EnemyScan.Mode.Charging;
                    //ea.es.markerSet.transform.localRotation = Quaternion.identity;
                    ea.Prepping(1);
                    activeSkill = ea.Rigid(new Vector2(Mathf.Sign(x)*3.5f,0f),selection,0,false);// Fireball Barrage
                }
                else
                {
                    activeSkill = ea.Tracker(4);
                    ea.desync = true;
                }
            }

    }
    void Start()
    {
        //ea = GetComponent<EnemyAI>();
        firebasePos = fireballSet.transform.GetChild(0).localPosition;
        od = GetComponent<Overdrive>();
    }

    void ProjectileFire(int i)
    {
        GameObject g = fireballSet.transform.GetChild(i).gameObject;
        SpriteRenderer sr = g.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        float scale = Mathf.Sign(ea.DistanceX());
        ProjectileScript ps = g.GetComponent<ProjectileScript>();
        fireballSet.transform.position = new Vector2(ea.transform.position.x + (3.5f*scale),ea.transform.position.y);
        g.transform.localPosition = Vector2.zero;

        if (scale <= -1f)
        {
            sr.flipX = false;
        }
        else
            sr.flipX = true;
        
        ps.modifier = 1.1f;
        //g.transform.position = new Vector2(-2.89f,6.12f);
        g.SetActive(true);
        switch(i)
        {
            case 0:
                g.transform.eulerAngles = new Vector3(0f,0f,-30f * scale * -1);
                ps.Fire(new Vector2(scale*12f,3f),1f);
                break;
            case 1:
                g.transform.eulerAngles = new Vector3(0f,0f,-15f * scale * -1);
                ps.Fire(new Vector2(scale*12f,1.5f),1f);
                break;
            case 2:
                g.transform.eulerAngles = new Vector3(0f,0f,0f);
                ps.Fire(new Vector2(scale*12f,0f),1f);
                break;
            case 3:
                g.transform.eulerAngles = new Vector3(0f,0f,15f * scale * -1);
                ps.Fire(new Vector2(scale*12f,-1.5f),1f);
                break;
            case 4:
                g.transform.eulerAngles = new Vector3(0f,0f,30f * scale * -1);
                ps.Fire(new Vector2(scale*12f,-3f),1f);
                break;

        }

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ea.es.mode == EnemyScan.Mode.Searching && od.state != Overdrive.StressState.Break)
        {
            SkillSel();
        }

        if (od.state == Overdrive.StressState.Neutral && od.armormode != Overdrive.ArmorType.SuperArmor)
            od.changeArmor(2);
    }
}
