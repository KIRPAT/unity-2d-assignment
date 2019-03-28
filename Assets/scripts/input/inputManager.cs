using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputManager : MonoBehaviour
{
    //Actions
    private bool left, right, jump, switchCharacter;
    //Device-Type
    private bool isPc; 
 
    void Start(){
        SetAllInputs(false);
    }

    void Update(){
        IsPcChecker();
        KeyboardInputHandler();
    }
    

    void IsPcChecker(){
        /* 
        Normally I would go with Input.anyKey to test for PC inputs, 
        but while testing the buttons on PC with mouse clicks,
        we are setting this value to true unintentionally. 
        */
        if(Input.GetAxis("Horizontal") !=0 || Input.GetAxis("Jump") !=0 || Input.GetAxis("Fire1") !=0){ 
            SetIsPc(true);             
        }
    }

    public void MakeUIActive() => SetIsPc(false);

    // Getters
    public bool IsLeft() => left;
    public bool IsRight() => right;
    public bool IsJump() => jump;
    public bool IsSwitchCharacter() => switchCharacter;
    // Setters
    public void SetLeft(bool isActive) => left = isActive; 
    public void SetRight(bool isActive) => right = isActive;
    public void SetJump(bool isActive) => jump = isActive;
    public void SetSwicthCharacter(bool isActive) => switchCharacter = isActive; 
    public void SetIsPc(bool isPc) => this.isPc = isPc;
    public void SetAllInputs(bool state){
        left = state;
        right = state;
        jump = state;
        switchCharacter = state;
    }

    private void KeyboardInputHandler(){
        if(isPc){  
            if (Input.GetAxis("Horizontal") > 0) SetRight(true);
            if (Input.GetAxis("Horizontal") <= 0) SetRight(false);
            if (Input.GetAxis("Horizontal") < 0) SetLeft(true);
            if (Input.GetAxis("Horizontal") >= 0) SetLeft(false);
            if (Input.GetAxis("Jump") > 0) SetJump(true); else SetJump(false);
            if (Input.GetAxis("Fire1") > 0) SetSwicthCharacter(true); else SetSwicthCharacter(false); 
        }
    }
}
