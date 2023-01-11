using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class NormalAttack : MonoBehaviour
{
    public Animator anim;
    
    public float attackRange = 1f;
    
    // AnimatorOverrideController
    public AnimatorOverrideController aoc;
    //public Dictionary<string,AnimationClip> attacks = new Dictionary<string,AnimationClip>();
    //public List<string> animname = new List<string>();
    // List of animation clips for Attacks and Recovery
    public List<AnimationClip> AttackAnim = new List<AnimationClip>();
    public List<AnimationClip> AttackRec = new List<AnimationClip>();

    //public List<AnimationClip> skillAnim = new List<AnimationClip>();
    
    public Beatemupmove bm;
    [SerializeField]private SkillPallete sp;
    //public Shadowpuppetry sp;
    private bool delay,airborne, skilldelay, sp_call = false;
    [HideInInspector]public int attack = 0;

    public float airheight = 0f;

    public Coroutine aerial = null;

    public int chain, maxaChain, maxChain;
    // gIndex = Ground Attack Index, gRecIndex = Ground Recovery Index
    public int aIndex = 0,aRecIndex = 0, gIndex = 0, gRecIndex = 0;

    public int skillsel = 0;

    public static NormalAttack instance;
    public GameObject kunai;
    private Vector3 slap;
    [SerializeField]NAAnims naa;
    [SerializeField]PlayerParty pp;

    private bool casting = false;
    private bool canNormal = true;
    // Start is called before the first frame update

    public bool a_air
    {
        get {return airborne; }
        set {airborne = value; }
    }

    public bool a_delay
    {
        get {return delay; }
        set {delay = value; }
    }

    public bool a_skilldelay
    {
        get {return skilldelay; }
        set {skilldelay = value; }
    }

    public bool a_sp
    {
        get{return sp_call;}
        set{sp_call = value;}
    }
    public bool a_canNormal
    {
        get{return canNormal;}
        set{canNormal = value;}
    }

    public void Reinitialize()
    {
        naa = pp.a_party[pp.active].transform.GetChild(0).GetComponent<NAAnims>();
        anim = pp.a_party[pp.active].transform.GetChild(0).GetComponent<Animator>();
        bm = pp.a_party[pp.active].GetComponent<Beatemupmove>();

        if (naa == null)
            Debug.Log("Couldn't get normal attack anims");
        anim.runtimeAnimatorController = naa.aoc;
        aoc = naa.aoc;

        AttackAnim.Clear();
        AttackRec.Clear();

        foreach (AnimationClip anim in naa.AttackAnim)
        {
            AttackAnim.Add(anim);
        }

        foreach (AnimationClip anim in naa.AttackRec)
        {
            AttackRec.Add(anim);
        }

        gIndex = 0;
        gRecIndex = 0;
        aIndex = naa.NAcount;
        aRecIndex = naa.NAcount;
        maxChain = naa.NAcount;
        maxaChain = naa.NAcount + naa.AAcount - 1;

    }

    void Start()
    {
        delay = false;
        Reinitialize();
        //aoc["Void_Hold"] = skillAnim[0];
        //aoc["DarkThunderBlade"] = skillAnim[1];
        /**
        for (int i = 0; i < animname.Count;i++)
        {
            attacks.Add(animname[i],AttackAnim[i]);
        }
        **/
    }

    public void PlayGenericSkill(List<AnimationClip> animset)
    {
        
        if (bm.a_onground)
        {
        
        aoc["NA1"] = animset[0];
        aoc["Rec1"] = animset[1];
        
        if (chain > 0)
            chain = 0;
        
            anim.Play("Skill");
            //skillsel = 0;
            //casting = false;
        }
        
        
    }

    // Once the state machine reads into the request it will read the current skill selection
    /**
    public void EvaluateSkill()
    {
        Skill s;
        if (bm.a_onground)
        {
            //Debug.Log("Skill array is "+skillsel - 1);
            if (skillsel - 1 < 0)
                return;
            s = sp.groundskills[skillsel - 1];
            // Assuming it's not blank will check if it's a special skill, otherwise play standard two state skill
            if (s != null)
            {
                if (!s.special)
                {
                    PlayGenericSkill(s.attackAnim);
                }
            }
            else
            {
                skillsel = 0;
            }
        }
    }
    **/

    public void PlayAnimation()
    {
        
        if (bm.a_onground)
        {
            //Debug.Log("Chain is "+chain);
            //bm.a_canmove = false;
            //Debug.Log("On ground");
            // Replace Normal Attack animation with indexed animation and recovery
            // Uses the starting index of the attack animation and progresses through via the chain
            // There's always an equal amount of attack animations as there are recovery
            if (gIndex + chain > AttackAnim.Count - 1)
            {
                chain = 0;
                return;
            }
            aoc["NA1"] = AttackAnim[gIndex + chain];
            aoc["Rec1"] = AttackRec[gRecIndex + chain];
            chain++;
            if (chain == maxChain)
            {
                chain = 0;
            }
        }
        else
        {
            if (aIndex + chain > AttackAnim.Count - 1)
            {
                chain = 0;
                return;
            }
            aoc["NA1"] = AttackAnim[aIndex + chain];
            aoc["Rec1"] = AttackRec[aRecIndex + chain];
            chain++;
            if (chain == maxaChain)
                chain = 0;
        }
        //Debug.Log("Chain now is "+chain);
        anim.Play("NA");
        bm.a_canmove = false;
    }

    private void Awake()
    {
        instance = this;
    }

    public void Mobility()
    {
        bm.a_canmove = true;
    }

    public void Readyup()
    {
        delay = false;
    }

    /**

    void FireKunai(int kunais)
    {
        int i;
        float start = 0.25f;
        Vector2 basepos = positioning(),startpos;
        ProjectileScript ps = kunai.GetComponent<ProjectileScript>();

        if (sp.sr.flipX)
            ps.rightward = false;
        else
            ps.rightward = true;
        
        for (i = 0; i < kunais; i++)
        {
            startpos = new Vector2(basepos.x,basepos.y + start);
            Instantiate(kunai,startpos,Quaternion.identity);
            start -= 0.25f;
        }
    }
    **/

    void AttackForce(float power)
    {
        if (!bm.a_flip)
            bm.a_party_rb.velocity = new Vector2(power,0f);
        else
            bm.a_party_rb.velocity = new Vector2(-power,0f);
    }

    void AttackStop()
    {
        //sp.sleeping = true;
        bm.a_party_rb.Sleep();
    }


    private IEnumerator Cooldown(float waitTime, int vardelay)
    {
        yield return new WaitForSeconds(waitTime);

        switch(vardelay)
        {
            // Airborne
            case 1:
                //bm.SuspendFalling(false);
                //aerial = null;
                //sp.shadowb.isKinematic = false;
                break;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        //skeleton.transform.position = pos;
        /**
        if (airborne)
        {
            //Debug.Log("Air time");
            if (!sp.dashed)
                sp.puppet.transform.position = new Vector2(sp.puppet.transform.position.x,airheight);
            else
                airheight = sp.puppet.transform.position.y;
        }
        **/

        //Use this block for ground check
        

        // As normal attack delay only ends once player gets out of the two state period
        // Player can't use normal attacks until they recover from their recent skill
        if (Input.GetKeyDown("z") && !delay && canNormal)
        {
            delay = true;
            attack = 1;
            Beatemupmove bm = pp.a_party[pp.active].GetComponent<Beatemupmove>();
            if (!bm.a_onground)
            {
                if (aIndex + chain < maxaChain)
                    bm.SuspendFalling(anim.transform.localPosition,AttackAnim[aIndex + chain].length + 0.5f);
            }
            //skilldelay = true;
        }
        

        // While selection of skill is 0, player can input any of these combo of buttons to trigger skill
        // If casted by Cecile, refer to CecileCore.cs for further inquiry
        
        /**
        if (skillsel != 0 && !skilldelay)
        {
            Debug.Log("Skill activate!");
            attack = 2;
            skilldelay = true;
        }
        **/

        
        
    }
}
