using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAtk : PlayerStateAction {
    private float _lastNormalizedTime;
    private bool moved;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        LoadComponents(animator);
        if (!_enabled) {
            return;
        }

        SetBool("atk01_" + _weaponAnim, false, false);

        if(TargetManager.Instance.HasAttackTarget()) {
            PlaySoundAtRatio(CharacterSoundEvent.Atk_1H, _audioHandler.AtkRatio);
            PlaySoundAtRatio(ItemSoundEvent.sword_small, _audioHandler.SwishRatio);
            PlayerController.Instance.SetCanMove(false);
            PlayerCombatController.Instance.AutoAttacking = true;
        }

        _lastNormalizedTime = 0;
        moved = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (!_enabled) {
            return;
        }

        if (!moved) {
            if ((stateInfo.normalizedTime - _lastNormalizedTime) >= 1f) {
                _lastNormalizedTime = stateInfo.normalizedTime;
                PlaySoundAtRatio(CharacterSoundEvent.Atk_1H, _audioHandler.AtkRatio);
                PlaySoundAtRatio(ItemSoundEvent.sword_small, _audioHandler.SwishRatio);
            }

            if ((stateInfo.normalizedTime % 1) <= 0.50f) {
                PlayerController.Instance.SetCanMove(false);
            } else { 
                if ((InputManager.Instance.IsInputPressed(InputType.Move) || PlayerController.Instance.RunningToDestination)) {
                    PlayerController.Instance.SetCanMove(true);
                }
                moved = ShouldRun();
            }

            if ((stateInfo.normalizedTime % 1) >= 0.90f) {
                if (!TargetManager.Instance.HasAttackTarget() || TargetManager.Instance.AttackTarget.Data.Entity.IsDead()) {
                    PlayerEntity.Instance.StopAutoAttacking();
                }
            }
        }

        ShouldAttack();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        PlayerCombatController.Instance.AutoAttacking = false;
        PlayerEntity.Instance.StopAutoAttacking();
        PlayerController.Instance.SetCanMove(true);
    }
}