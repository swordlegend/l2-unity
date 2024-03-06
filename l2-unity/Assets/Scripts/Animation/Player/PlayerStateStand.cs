using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateStand : PlayerStateBase {
    private float _lastNormalizedTime = 0;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        LoadComponents(animator);
        if (!_enabled) {
            return;
        }

        SetBool("stand", false, false);
        _lastNormalizedTime = 0;
        _audioHandler.PlaySound(CharacterSoundEvent.Standup);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (!_enabled) {
            return;
        }

        if ((stateInfo.normalizedTime - _lastNormalizedTime) >= 1f) {
            _lastNormalizedTime = stateInfo.normalizedTime;
            SetBool("wait_" + _weaponAnim, true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (!_enabled) {
            return;
        }

        CameraController.Instance.StickToBone = false;
        PlayerController.Instance.SetCanMove(true);
    }
}