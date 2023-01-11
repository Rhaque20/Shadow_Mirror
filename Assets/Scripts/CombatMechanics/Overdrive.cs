using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overdrive : InterruptSystem
{
    public EnemyVariables ev;
    public enum StressState {Neutral,Overdrive,Break};
    [SerializeField]StressState ss = StressState.Neutral;
    float stress = 0.0f;
    [SerializeField]EnemyAI ea;
    public bool hasOverdrive = false;
    private float toOD = 0f, toBreak = 0f;
    [SerializeField]float toODscale = 0.25f, toBreakscale = 0.4f;
    StatusChanges sc;
    [SerializeField]private int stoicThreshold = 0;
    private int hitCount = 0;
    [SerializeField]Charger c;
    EnemyMobile em;
    SpriteEmissionManager sem;

    public StressState state
    {
        get {return ss;}
    }
    
    public float modeval
    {
        get {return stress;}
    }

    public float toODmax
    {
        get{return toOD;}
    }

    public float toBreakmax
    {
        get{return toBreak;}
    }

    public void changeArmor(int i)
    {
        switch(i)
        {
            case 1:
                armor = ArmorType.Neutral;
                break;
            case 2:
                armor = ArmorType.SuperArmor;
                break;
            default:
                armor = ArmorType.HyperArmor;
                break;
        }
    }

    public override IEnumerator Flinch(float staggertime)
    {
        yield return new WaitForSeconds(staggertime);
        anim.SetTrigger("recover");
        inStagger = null;
        flinchMode = FlinchType.Neutral;
        ResumeMotion(false);
    }

    private IEnumerator BreakDuration(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ss = StressState.Neutral;
        armor = ArmorType.SuperArmor;
        aoc["Idle"] = statusAnim[0];// Re-replace the break idle with normal idle
        PlayStatusShift(6);// Play Recover animation
        ResumeMotion(false);

    }
    
    public void PlayStatusShift(int i)
    {
        aoc["StatusShift"] = statusAnim[i];
        anim.runtimeAnimatorController = aoc;
        anim.Play("StatusShift");
    }

    public void ActivateOverdrive()
    {
        ss = StressState.Overdrive;
        //ea.s.ModeStatShift();
        armor = ArmorType.HyperArmor;
        sc.ClearAllType(true);
        ea.InterruptAttack();
        ea.em.movable = false;
        PlayStatusShift(3);// Activate Enrage
        if (sem != null)
            sem.GradEmit = true;
    }
    
    public void ActivateBreak()
    {
        armor = ArmorType.Neutral;
        ss = StressState.Break;
        ea.InterruptAttack();
        aoc["Idle"] = statusAnim[5]; // Replace default idle with break idle
        PlayStatusShift(4);// Activate Break animation
        //ea.InterruptAttack(statusAnim[2]);
        ev.movement.movable = false;
        StartCoroutine(BreakDuration(10f));
        if (sem != null)
            sem.DeactiveEmission();
    }
    public void ResumeMotion()
    { 
        if (ss != StressState.Break)
        {
            ev.movement.movable = true;
            ea.es.mode = EnemyScan.Mode.Recover;
        }
    }
    
    // Renables enemy movement
    public void ResumeMotion(bool revert)
    {   
        if (ss != StressState.Break)
        {
            ev.movement.movable = true;
            ea.es.mode = EnemyScan.Mode.Recover;

            if (hasOverdrive)
            {
                hitCount = 0;
                return;
            }
            
            if (stress >= toOD || hitCount == stoicThreshold)
            {
                hitCount = 0;
                if (!hasOverdrive)
                    stress = 0f;
                armor = ArmorType.SuperArmor;
                Debug.Log("STOIC ACTIVATE!");
            }
            else
                armor = ArmorType.Neutral;
        }

        if (revert)
            armor = ArmorType.Neutral;
    }

    public void StressFill(float stressdmg)
    {
        float multiplier = 0f;

        if (ss != StressState.Break && hasOverdrive)
        {
            if (ss == StressState.Overdrive)
                multiplier = s.a_As.ODx;
            if (ss == StressState.Neutral)
                multiplier = s.a_As.Normalx;
            
            if (s.sc.eff.ContainsKey("Paralyze"))
            {
                StatusEffect status = s.sc.eff["Paralyze"];
                multiplier +=  status.effect[status.chain];
            }

            
            if (multiplier > 0f)
            {
                if (stressdmg > 0f && ss == StressState.Overdrive)
                    stressdmg *= -1;
            }

            stress += stressdmg * multiplier;
            // This will trigger the overdrive state as well as prevent overflow
            if (stress >= toOD && ss == StressState.Neutral)
            {
                stress = toBreak;
                ActivateOverdrive();
            }
            // This will trigger the break state if target was in overdrive
            if (stress <= 0.0f)
            {
                stress = 0.0f;
                if (ss == StressState.Overdrive)
                {
                    ActivateBreak();
                }
            }
        }
    }

    public override void TriggerStagger()
    {
        ea.InterruptAttack(statusAnim[1]);
        /**
        if (isRight)
        {
            force *= new Vector2(-1f,1f);
        }
        **/
        //c.Knockback(force);
        if (inStagger != null)
            StopCoroutine(inStagger);
        inStagger = StartCoroutine(Flinch(0.5f));
        flinchMode = FlinchType.Knockback;
    }
    
    public override void TriggerKOAnim()
    {
        ea.InterruptAttack(statusAnim[2]);
    }

    public void stoicHit(float damage)
    {
        if (armor != ArmorType.Neutral)
            return;
        
        hitCount++;

        if (!hasOverdrive)
            stress += damage;
        
        if (armor == ArmorType.Neutral)
            TriggerStagger();
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //s = ea.s;
        aoc["Idle"] = statusAnim[0];
        toOD = s.maxHP * toODscale;
        toBreak = s.maxHP * toBreakscale;
        sc = s.sc;
        sem = GetComponent<SpriteEmissionManager>();
        if (sem != null)
            sem.DeactiveEmission();
        //em = ev.movement;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
