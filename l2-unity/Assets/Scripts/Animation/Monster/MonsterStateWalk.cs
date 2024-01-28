using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateWalk : MonsterStateBase
{
    float lastNormalizedTime = 0;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        LoadComponents(animator);
        audioHandler.PlaySoundAtRatio(MonsterSoundEvent.Step, 0.25f); 
        audioHandler.PlaySoundAtRatio(MonsterSoundEvent.Step, 0.75f);
        lastNormalizedTime = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        // Check if the state has looped (re-entered)
        if((stateInfo.normalizedTime - lastNormalizedTime) >= 1f) {
            // This block will be executed once when the state is re-entered after completion
            lastNormalizedTime = stateInfo.normalizedTime;
            audioHandler.PlaySoundAtRatio(MonsterSoundEvent.Step, 0.25f);
            audioHandler.PlaySoundAtRatio(MonsterSoundEvent.Step, 0.75f);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    }
}
