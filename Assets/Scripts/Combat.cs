using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    // Start is called before the first frame update
    public static Combat instance;
    public bool delay = false;
    public int attack = 0;
    public Shadowpuppetry sp;
    public int chain, aChain, maxChain;
    // gIndex = Ground Attack Index, gRecIndex = Ground Recovery Index
    public int aIndex = 0,aRecIndex = 0;
    public List<AnimationClip> animload = new List<AnimationClip>();
    public AnimatorOverrideController aoc;
    public Animator anim;
    void Start()
    {
        
    }

    void Awake()
    {
        instance = this;
    }

    public void Mobility()
    {
        sp.canmove = true;
    }

    void Reset()
    {
        chain = 0;
    }

    public void PlayAnimation()
    {
        aoc["TestAttack1"] = animload[aIndex + chain];
        aoc["Transition1"] = animload[aRecIndex + chain];
        anim.runtimeAnimatorController = aoc;
        anim.Play("AA");
        chain++;

        if (chain == maxChain)
            chain = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z") && !delay)
        {
            delay = true;
            attack = 1;
            //Debug.Log("And swing!");
            sp.canmove = false;

            
        }
    }
}
