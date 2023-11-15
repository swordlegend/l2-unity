﻿using System.Collections;
using UnityEngine;

public enum State {
    Running,
    AfterRun,
    Idle,
    Jumping,
    Dodging,
    Attacking,
    WaitingAttack,
    Blocking,
    WeaponSwitching,
    SmoothJumping,
    Walking,
    Waiting,
    Dead
}

public class AnimationController : MonoBehaviour {

    private Animator animator;
    private PlayerController pc;

    public bool local;

    void Start() {
        animator = GetComponent<Animator>();
        pc = transform.parent.GetComponent<PlayerController>();
    }

    void Update() {
        UpdateAnimator();
    }

    void UpdateAnimator() {

        SetFloat("Speed", pc.currentSpeed);

        /*Jump */
         if(InputManager.GetInstance().IsInputPressed(InputType.Jump) && IsCurrentState("Idle")) {
            SetBool("IdleJump", true);
        } else {
            SetBool("IdleJump", false);
        }

        /*Jump */
        if(InputManager.GetInstance().IsInputPressed(InputType.Jump) && IsCurrentState("Run") || IsCurrentState("RunJump") && !IsAnimationFinished(0)) {
            SetBool("RunJump", true);
        } else {
            SetBool("RunJump", false);
        }

        /* Run */
        if(InputManager.GetInstance().IsInputPressed(InputType.InputAxis) && (IsCurrentState("Idle") || IsAnimationFinished(0)) && pc.canMove) {
            SetBool("Moving", true);
        } else {
            SetBool("Moving", false);
        }

        if(InputManager.GetInstance().IsInputPressed(InputType.Sit) && (IsCurrentState("Run") || IsCurrentState("Idle"))) {
            pc.canMove = false;
            SetBool("Sit", true);
        } else {
            SetBool("Sit", false);
        }

        if(IsCurrentState("SitTransition") && IsAnimationFinished(0)) {
            SetBool("SitWait", true);
        }

        if((InputManager.GetInstance().IsInputPressed(InputType.Sit) || InputManager.GetInstance().IsInputPressed(InputType.InputAxis)) 
            && (IsCurrentState("SitWait"))) {
            SetBool("Stand", true);
            SetBool("SitWait", false);
        } else {
            SetBool("Stand", false);
        }

        if(IsCurrentState("Stand") && IsAnimationFinished(0)) {
            pc.canMove = true;
        }

        if(!InputManager.GetInstance().IsInputPressed(InputType.InputAxis) 
            && (IsCurrentState("Run") || IsAnimationFinished(0)) 
            && !IsCurrentState("SitTransition") 
            && !IsCurrentState("Sit")
            && !IsCurrentState("SitWait")) {
            SetBool("Idle", true);
        } else {
            SetBool("Idle", false);
        }
    }

    private bool IsAnimationFinished(int layer) {
        return animator.GetCurrentAnimatorStateInfo(layer).normalizedTime > 0.95f;
    }

    private bool IsCurrentState(string state) {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(state);
    }

    void SetBool(string name, bool value) {
        if(animator.GetBool(name) != value) {
            animator.SetBool(name, value);
            //if(!local)
            //	syncAnim.EmitAnimatorInfo (name, value.ToString());
        }
    }

    void SetFloat(string name, float value) {
        if(Mathf.Abs(animator.GetFloat(name) - value) > 0.2f) {
            animator.SetFloat(name, value);
            //	if (!local) 
            //	syncAnim.EmitAnimatorInfo (name, (Mathf.Floor((value+0.01f)*100)/100).ToString ());
        }
    }

    void SetInteger(string name, int value) {
        if(animator.GetInteger(name) != value) {
            animator.SetInteger(name, value);
            //if (!local) 
            //	syncAnim.EmitAnimatorInfo (name, value.ToString ());
        }
    }

    void SetTrigger(string name) {
        animator.SetTrigger(name);
        //if (!local)
        //	syncAnim.EmitAnimatorInfo (name, "");
    }
}


