using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bob : PlayerController
{
    public float smashSpeed;
    private bool isSmashAllowed;
    private bool isSmashActive;

    override public void SpecialMovePropertyInitializer(){ // -> void Start()
        isSmashActive = false;
        isSmashAllowed = true;
    }

    override public void SpecialMoveTrigger(){ // -> void Update()
        if(Input.GetKeyDown(KeyCode.Space) && !isGrounded && isSmashAllowed) {
            isSpecialMoveTriggered = true;       
        }
    }
    override public void SpecialMoveHandler(){// -> void Update()
        if (isSpecialMoveTriggered){ 
            Smash();
        }
    }
    private void Smash(){
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
