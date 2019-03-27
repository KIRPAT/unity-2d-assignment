using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bob : PlayerController
{
    public float smashSpeed;
    private bool isSmashAllowed;
    private bool isSmashActive;

    override public void SpecialMovePropertyInitializer(){ // -> void Start()
    }

    override public void SpecialMoveTrigger(){ // -> void Update()
        if(Input.GetKeyDown(KeyCode.Space) && !isGrounded) {
            isSpecialMoveTriggered = true;       
        }
    }
    override public void SpecialMoveHandler(){// -> void Update()
        if (isSpecialMoveTriggered){ 
            Smash();
        }
    }
    private void Smash(){
        isSpecialMoveTriggered = false;
        playerRigidBody.velocity = Vector2.down * smashSpeed;
    }
    
}
