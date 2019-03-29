using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bob : PlayerController{
    public float smashSpeed;
    bool isSmashAllowed;
    bool isSmashActive;
    override public void SpecialMovePropertyInitializer(){ // -> void Start()
        isSmashActive = false;
        isSmashAllowed = true;
    }
    override public void SpecialMoveTriggerListener(){ // -> void Update()
        if(inputManager.IsJump() && !isGrounded && isSmashAllowed && !jumpActionBlocker) {
            isSpecialMoveTriggered = true;       
        }
    }
    override public void SpecialMoveHandler(){// -> void Update()
        if (isSpecialMoveTriggered) Smash();
    }
    void Smash(){
        if (!isSmashActive){
            isSmashActive = true;
            isSmashAllowed = false; //Prevents multiple smashing in the air.
        } else {
            if(isGrounded){
                isSmashActive = false;
                isSpecialMoveTriggered = false;
                isSmashAllowed = true;
            } else {
                playerRigidBody.velocity = Vector2.down * smashSpeed;
            }
        }
    }
}
