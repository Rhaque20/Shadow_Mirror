using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleScript : StateMachineBehaviour
{
    bool ready = true;
    public NormalAttack ac;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //NormalAttack.instance.a_skilldelay = false;
        /**
        if (NormalAttack.instance.attack == 2)
        {
            NormalAttack.instance.EvaluateSkill();
        }
        **/

        if(NormalAttack.instance.a_delay && ready)
        {
            NormalAttack.instance.chain = 0;
            Debug.Log("Calledo n idlescript");
            //Debug.Log(NormalAttack.instance.natk_name+nexatk.ToString());
            //NormalAttack.instance.anim.Play(NormalAttack.instance.natk_name+nexatk.ToString());
            NormalAttack.instance.PlayAnimation();
            ready = false;

        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Called on idle exit");
        NormalAttack.instance.a_delay = false;
        ready = true;
        //NormalAttack.instance.Mobility();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
