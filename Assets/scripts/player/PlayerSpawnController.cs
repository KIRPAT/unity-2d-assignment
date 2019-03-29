using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
I know that you explicitly wanted me to create two scenes for each character.
Then I thought of the game "Trine", where you can switch between theree characters on the fly. 
So.. I have implemented that swicth mechanic instead. Having the same scene twice looked like redundant. 
*/ 

public class PlayerSpawnController : MonoBehaviour {
    private inputManager inputManager;
    public GameObject alicePrefab, bobPrefab;
    private GameObject activePlayer;
    public bool currentlyActivePlayer = true; // t: Alice, f: Bob
    private bool actionBlocker, coolDownBlocker;
    //Cooldown
    public float coolDownTime;
    private float coolDownTimeTemp;
    
    void Start(){
        coolDownBlocker = true;
        actionBlocker= true;
        coolDownTimeTemp = coolDownTime;
        PlayerSpawner();
        inputManager = GetComponent<inputManager>();
    }
    void Update(){
        PlayerSwitchHandler();
    }
    void PlayerSpawner(){
        activePlayer = PlayerInstantiator(currentlyActivePlayer ? "Alice":"Bob");
        activePlayer.transform.parent = gameObject.transform;
    }
    
    GameObject PlayerInstantiator(string playerName){
        switch (playerName){
            case "Alice": return Instantiate(alicePrefab) as GameObject; 
            case "Bob": return Instantiate(bobPrefab) as GameObject;
            default: return null; 
        }
    }
    void PlayerSwitchHandler() {
        /*  
          This method is similar to Dash(), Alice's Ability. 
          This time, the PlayerSwitch() needs to be executed once and waits for the cooldown. 
          We do not want to constantly switch again, and again... while the cooldown is active.
          Since I'm not using Input.GetKeyDown here, 
          I had to block the skill activasion with Cooldowns.
        */ 
        if (inputManager.IsSwitchCharacter() && coolDownBlocker){
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
    void PlayerSwicth(){
        Transform currentPlayerTransform = activePlayer.transform;
        Destroy(activePlayer);
        currentlyActivePlayer = !currentlyActivePlayer;
        PlayerSpawner();
        activePlayer.transform.position = currentPlayerTransform.position;
    }
    bool IsSwicthKeyReleased() => !inputManager.IsSwitchCharacter() ? true : false;  

}
