using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTransition : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
    // Throughout the entirety of this state, delay is still enabled
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /**
        if (NormalAttack.instance.attack == 0)
        {
            Debug.Log("Dead end chief.");
            if (NormalAttack.instance.delay)
                Debug.Log("How that happen?");
        }
        **/


        //NormalAttack.instance.a_skilldelay = false;
        /**
        if (NormalAttack.instance.attack == 2 && NormalAttack.instance.a_skilldelay)
        {
            Debug.Log("Chain!");
            NormalAttack.instance.EvaluateSkill();
        }
        **/

        //Debug.Log("Delay status "+NormalAttack.instance.a_delay);

        if(NormalAttack.instance.a_delay && NormalAttack.instance.attack == 1)
        {
            Debug.Log("Calledo n attacktransition "+NormalAttack.instance.chain);
            //NormalAttack.instance.chain = 0;
            //Debug.Log(NormalAttack.instance.natk_name+nexatk.ToString());
            //NormalAttack.instance.anim.Play(NormalAttack.instance.natk_name+nexatk.ToString());
            NormalAttack.instance.PlayAnimation();

        }

    }
    // Once the play stops giving additional input it will end delay
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        NormalAttack.instance.a_delay = false;
        NormalAttack.instance.a_skilldelay = false;
        //NormalAttack.instance.skillsel = 0;
        
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
