using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemyVariables ev;
    public EnemyScan es;
    public Animator anim;
    public EnemyStats s;
    public EnemyMobile em;
    public Charger c;
    private EnemyScan.AimType at = EnemyScan.AimType.rigid;
    [SerializeField]private Overdrive od;

    private Coroutine delayHolder;
    private bool async = false;

    [SerializeField]private bool starting = false,ramattack = false;
    [SerializeField]private int selection = 2;
    [SerializeField]PlayerParty pp;
    [SerializeField]GameObject gp;//Groundpoint
    [SerializeField]EnemySkill[] skills;
    EnemySkill sel;
    [SerializeField]private AnimatorOverrideController aoc;
    [SerializeField]EnemyCore ec;

    float delayValue = 1f;

    private int amc = 1;// AttackMarkerCount, how many attack markers are involved


    public bool desync
    {
        get{return async;}
        set{async = value;}
    }

    public Overdrive mode
    {
        get{return od;}
    }

    private IEnumerator ManageCooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        es.mode = EnemyScan.Mode.Searching;
        delayHolder = null;

    }

    public void Prepping(int numMarks)
    {
        es.mode = EnemyScan.Mode.Charging;
        amc = numMarks;
        starting = false;
    }

    public void LoadUp()
    {
        ec = ev.ec;
        es.s = s;
        //od.a_stat = s;
        ec.s = s;
        em.anim.runtimeAnimatorController = aoc;
        anim = em.anim;
        es.party = pp;
    }

    void Start()
    {
        s = GetComponent<EnemyStats>();
        selection = 0;
        //defaultposition = es.a_t.transform.localPosition;
        /**
        selection = Random.Range(1,3);

        switch(selection)
        {
            case 1:
                es.aim = EnemyScan.AimType.rigid;
                break;
            case 2:
                es.aim = EnemyScan.AimType.freeAim;
                break;
        }
        **/
    }

    // This creates the telegraph for the attack and essentially starts the attack
    public void SummonTelegraphs(int i, int j)
    {
        if (i < 0 || i > es.a_t.Length)
            return;
        
        if (es.a_t[i].aim)
            es.a_t[i].TelegraphSetup(sel.strong[j],sel.getATKSPD() - 0.01f,true, sel.showTelegraph);
        else
            es.a_t[i].TelegraphSetup(sel.strong[j],sel.getATKSPD() - 0.01f,sel.showTelegraph);
    }

    public float DistanceX()
    {
        //Debug.Log(Mathf.Abs(gp.transform.position.x - pp.a_party[pp.active].transform.position.x));
        return pp.a_party[pp.active].transform.position.x - gp.transform.position.x;
    }

    public float DistanceY()
    {
        return pp.a_party[pp.active].transform.position.y - gp.transform.position.y;
    }

    public bool RangeX(float lowerx,float upperx, float xval)
    {
        return (xval < upperx && xval > lowerx);
    }
    public bool RangeY(float lowery,float uppery, float yval)
    {
        return (yval < uppery && yval > lowery);
    }

    public EnemySkill skillsel(int i)
    {
        aoc["NA1"] = skills[i].attackAnim[0];
        aoc["Rec1"] = skills[i].attackAnim[1];
        return skills[i];
    }

    public EnemySkill FreeAim(int i, bool isRam)
    {
        sel = skillsel(i);
        es.markerSet.transform.localRotation = Quaternion.identity;
        ec.skillsel = sel;
        es.InitalizeSize(sel.markerScales[0],0);
        //c.aim = true;
        es.aim = EnemyScan.AimType.freeAim;
        c.dc.a_es = sel;
        delayValue = 2f;
        ramattack = isRam;
        es.SetLayers(sel.targetZone);
        return sel;
    }

    public EnemySkill Rigid(Vector2 position, int i, int markersel, bool setTurn)
    {
        GameObject g;
        em.turnable = setTurn;
        es.markerSet.transform.localRotation = Quaternion.identity;
        g = es.markerSet.transform.GetChild(markersel).gameObject;
        g.transform.localPosition = position;
        sel = skillsel(i);
            
        if (position.x > 0)// Facing Left
        {
            if (g.transform.localScale.x > 0)
                es.InitalizeSize(sel.markerScales[0] * -1,markersel);
        }
        else// Facing Left
        {
            es.InitalizeSize(sel.markerScales[0],markersel);
        }
        es.SetLayers(sel.targetZone);
        //ec.skillsel = sel;
        //es.InitalizeSize(sel.markerScales[0],0);
        delayValue = 1f;
        es.aim = EnemyScan.AimType.rigid;
        return sel;
    }

    public EnemySkill Tracker(int i)
    {
        sel = skillsel(i);
        es.markerSet.transform.localRotation = Quaternion.identity;
        ec.skillsel = sel;
        es.InitalizeTracker(sel.markerScales[0],1);
        delayValue = 3f;
        es.aim = EnemyScan.AimType.tracker;
        async = true;
        es.SetLayers(sel.targetZone);
        return sel;
    }

    public void ReleaseAttack()
    {
        anim.SetTrigger("release");
        starting = false;
        es.mode = EnemyScan.Mode.Strike;
    }

    public void InterruptAttack()
    {   
        for (int i = 0; i < es.markerSet.transform.childCount; i++)
        {
            if (es.a_t[i].q != null)
            {
                Debug.Log("Canceling attack");
                es.a_t[i].StopTelegraph();// Stopping any coroutine telegraphs

            }
        }

        if (ramattack)
        {
            c.AttackRecover(true);
        }

        if (delayHolder != null)
        {
            StopCoroutine(delayHolder);
            delayHolder = null;
        }

        //em.movable = true;
        //es.mode = EnemyScan.Mode.Recover;
        //anim.Play("Idle");

    }
    // Interrupt any ongoing attack that the enemy queues up. Only called if triggering overdrive or if a status affliction stuns them
    public void InterruptAttack(AnimationClip animation)
    {
        InterruptAttack();
        em.movable = false;
        aoc["StatusShift"] = animation;
        anim.runtimeAnimatorController = aoc;
        anim.Play("StatusShift");
        if (delayHolder != null)
            StopCoroutine(delayHolder);
        delayHolder = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // This will initiate a delay countdown after an attack which will increase/decrease based on difficulty and paralysis
        if (delayHolder == null && es.mode == EnemyScan.Mode.Recover && od.state != Overdrive.StressState.Break)
        {
            delayHolder = StartCoroutine(ManageCooldown(delayValue));
        }
        
        //If an attack isn't already active and the enemy is readying their attack
        if (es.mode == EnemyScan.Mode.Charging && !starting)
        {
            Debug.Log("Starting attack");
            // Starts up the attack
            anim.SetTrigger("attack");
            //anim.SetInteger("skill",1);
            // Prevents the enemy from moving
            em.movable = false;
            // Attack is now active
            starting = true;
            // All attack twos will be charging attacks hence allowing for aim
            selection = 0;
        }

        /**
        if (es.mode == EnemyScan.Mode.Strike && em.turnable && es.aim == EnemyScan.AimType.rigid)
        {
            int counter = 0;
            for (int i = 0; i < es.markerSet.transform.childCount; i++)
            {
                if (es.a_t[i].q != null)
                {
                    //counter++;
                    Transform t = es.markerSet.transform.GetChild(i);
                    float xSign = Mathf.Sign(em.body.transform.localScale.x);
                    //Mathf.Sign(em.body.transform.localScale.x);
                    t.localPosition = new Vector2(t.localPosition.x * xSign, t.localPosition.y);
                    t.localScale = new Vector2(t.localScale.x *xSign, t.localScale.y);
                    if (counter == amc)
                        break;
                }
            }
        }
        **/
        
        if (amc == 1)
        {
            // Run through all the markers that were set and activate them
            for (int i = 0; i < es.markerSet.transform.childCount; i++)
            {
                if (es.a_t[i].r && starting)
                {
                    //anim.SetInteger("skill",0);
                    ReleaseAttack();
                    es.a_t[i].r = false;
                    if (amc == 1)
                    {
                        async = false;
                        ramattack = false;
                        break;
                    }

                }
            }
        }
    }
}
