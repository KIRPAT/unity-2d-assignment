﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
I know that you explicitly wanted me to create two scenes for each ccharacter.
Then I thouth of the game "Trine", where you can switch between characters theree on the fly. 
So.. I have implemented that swicth mechanic instead. Having the same scene twice looked like redundant.
I have also found a Brackeys 
*/ 

public class PlayerSpawnController : MonoBehaviour {
    public GameObject alicePrefab, bobPrefab;
    private GameObject activePlayer;
    public bool currentlyActivePlayer = true; // t: Alice, f: Bob
    private bool actionBlocker, coolDownBlocker, isSwitchKeyReleased;

    public float coolDownTime;
    private float coolDownTimeTemp;

    void Start(){
        coolDownBlocker = true;
        actionBlocker= true;
        coolDownTimeTemp = coolDownTime;
        PlayerSpawner();
        
    }
    void Update(){
        PlayerSwitchHandler();
    }
    private void PlayerSpawner(){
        activePlayer = PlayerInstantiator(currentlyActivePlayer ? "Alice":"Bob");
    }
    
    private GameObject PlayerInstantiator(string playerName){
        switch (playerName){
            case "Alice": return Instantiate(alicePrefab) as GameObject; 
            case "Bob": return Instantiate(bobPrefab) as GameObject;
            default: return null; 
        }
    }
    public void PlayerSwitchHandler() {
        // This method is similar to Dash(), Alice's Ability 
        // But this time, the PlayerSwitch() needs to be executed once and waits for the cooldown. 
        // We do not want to constantly switch again, and again... while the cooldown is active.
        // Since I'm not using Input.GetKeyDown here, 
        // I had to block the skill activasion with Cooldowns. 
        if (Input.GetAxis("Fire1") > 0 && coolDownBlocker){
            coolDownBlocker = false; //cooldown activated 
            actionBlocker = false; //action activated
        }
        if (!actionBlocker){ //makes sure the action is played only when triggered
            PlayerSwicth();    
            actionBlocker = true; // action is no longer allowed
        }
        if (!coolDownBlocker){
            coolDownTimeTemp -= Time.deltaTime;
            if (coolDownTimeTemp <= 0){ //If the cooldown is finished... 
                if (IsSwicthKeyReleased()){ // ...check if the button is still being pressed, (Without this check, as soon as the cooldown ends, and if you are still holding the Switch button, you will unintentionally execute SwitchPlayer() again.)  
                    coolDownBlocker = true; // ...if not, cooldown is blocked.
                }
                coolDownTimeTemp = coolDownTime;
            }
        }
    }
    private void PlayerSwicth(){
        Transform currentPlayerTransform = activePlayer.transform;
        Destroy(activePlayer);
        currentlyActivePlayer = !currentlyActivePlayer;
        PlayerSpawner();
        activePlayer.transform.position = currentPlayerTransform.position;
    }
    private bool IsSwicthKeyReleased() => Input.GetAxis("Fire1") == 0 ? true : false;  

}