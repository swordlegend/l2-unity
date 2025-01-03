using UnityEngine;

public class MonsterStateWait : MonsterStateBase
{
    public int playBreatheSoundChancePercent = 100;
    bool started = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        LoadComponents(animator);
        if(RandomUtils.ShouldEventHappen(playBreatheSoundChancePercent)) {
            audioHandler.PlaySound(MonsterSoundEvent.Wait);
        }
        SetBool("spwait", false);
        started = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if((stateInfo.normalizedTime % 1) >= 0.90f && started) {
            started = false;
            SetBool("wait", true);       
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }
}
