using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Alice : PlayerController{
    //DashProperties
    public float dashSpeed, dashTime;
    private float dashTimeTemp;
    public int allowedDashes;
    private int allowedDashesTemp;
    private bool isDashActive;
    public override void SpecialMovePropertyInitializer(){ // -> void Start()
        isDashActive = false;
        dashTimeTemp = dashTime;
        allowedDashesTemp = allowedDashes;
    }
    
    public override void SpecialMoveTrigger(){   
        //Without jumpActionBlocker, the dash skill gets activated as soon as we are of the ground.
        if (inputManager.IsJump() && !isGrounded && allowedDashesTemp > 0 && !jumpActionBlocker){
            allowedDashesTemp -= 1;
            isSpecialMoveTriggered = true;
        }
    }
    public override void SpecialMoveHandler(){
        if (isSpecialMoveTriggered){
            Dash();
        }
        if (isGrounded){
            allowedDashesTemp = allowedDashes;
        }
    }
    void Dash(){
        if (!isDashActive){ //Activates isDashActive, since it is active now, we can check if we need to deactiate it before even dashing. 
            isDashActive = true; //Keeps the dash active as long as it is true.
        } else {
            if (dashTimeTemp <= 0){ //Checks if the dashTimeTemp has ended. If it did, we need to deactivate dash time, set the the specialMoveTrigger status flase and reset the dashTimeTemp. 
                isDashActive = false; //Disables dash.
                isSpecialMoveTriggered = false; //Prevents Dash() from getting executed all over again when the Dash() lifecycle ends. This also gives the character controlls back to us.
                dashTimeTemp = dashTime; //Resets the dash timer.
                //I would normally add a line here to stop dash movement, but as soon as I give back character controls to KeyboardController() from the base class, it already does the job for me. 
            } else {
                dashTimeTemp -= Time.deltaTime;
                playerRigidBody.velocity = (currentCharacterDirection ? Vector2.right : Vector2.left) * dashSpeed;
            }
        }
    }
}

