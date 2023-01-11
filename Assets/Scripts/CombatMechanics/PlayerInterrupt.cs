using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterrupt : InterruptSystem
{
    [SerializeField]Beatemupmove bm;
    [SerializeField]CharacterCore charc;
    

    public override IEnumerator Flinch(float staggertime)
    {
        yield return new WaitForSeconds(staggertime);
        charc.SkillRecover();
        anim.SetTrigger("recover");
        inStagger = null;
        flinchMode = FlinchType.Neutral;
    }

    public override void TriggerStagger(Vector2 force, bool isRight)
    {
        //bm.a_canmove = false;
        if (armor != ArmorType.Neutral)
            return;
        
        charc.Immobilize();

        if (!isRight)
            force *= new Vector2(-1f,1f);
        bm.Knockback(force);
        
        aoc["Flinch"] = statusAnim[0];
        anim.Play("Stagger");

        if (inStagger != null)
            StopCoroutine(inStagger);
        
        inStagger = StartCoroutine(Flinch(0.5f));
        flinchMode = FlinchType.Knockback;

    }

    // Start is called before the first frame update
    void Start()
    {
       anim = GetComponent<Animator>(); 
       charc = GetComponent<CharacterCore>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
