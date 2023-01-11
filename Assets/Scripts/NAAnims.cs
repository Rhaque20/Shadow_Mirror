using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAAnims : MonoBehaviour
{
    public List<AnimationClip> AttackAnim = new List<AnimationClip>();
    public List<AnimationClip> AttackRec = new List<AnimationClip>();
    public AnimatorOverrideController aoc;
    private Animator anim;
    public int NAcount,AAcount;
    [SerializeField]NormalAttack na;
    Beatemupmove bm;

    public void Start()
    {
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = aoc;
        bm = na.bm;
    }

    public NormalAttack a_na
    {
        get{return na;}
    }

    public void Mobility()
    {
        bm = na.bm;
        Debug.Log("Called mobility! for "+bm.gameObject.name);
        bm.a_canmove = true;
    }

    public void PlayGenericSkill(List<AnimationClip> animset, bool QTE)
    {
        /**
        
        if (na.bm.a_onground)
        {
                //skillsel = 0;
                //casting = false;
        }
        **/
        
        aoc["AA2"] = animset[0];
        aoc["AirRec2"] = animset[1];
        
        if (na.chain > 0 && !QTE)
            na.chain = 0;
        
        anim.Play("Skill");
        
        
    }
}
