using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
I know that you explicitly wanted me to create two scenes for each character.
Then I thought of the game "Trine", where you can switch between theree characters on the fly. 
So.. I have implemented that switch mechanic instead.
*/ 

public class PlayerSpawnController : MonoBehaviour {
    InputManager inputManager;
    public GameObject alicePrefab, bobPrefab;
    GameObject activePlayer;
    public bool currentlyActivePlayer = true; // t: Alice, f: Bob
    bool actionBlocker, coolDownBlocker;
    //Cooldown
    public float coolDownTime;
    float coolDownTimeTemp;
    
    void Start(){
        coolDownBlocker = true;
        actionBlocker= true;
        coolDownTimeTemp = coolDownTime;
        PlayerSpawner();
        inputManager = GetComponent<InputManager>();
    }
    void Update() => PlayerSwitchHandler();
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
          Since I'm not using Input.GetKeyDown here, I had to block the skill activasion with Cooldowns.
        */ 
        if (inputManager.IsSwitchCharacter() && coolDownBlocker){
            coolDownBlocker = false; //cooldown activated 
            actionBlocker = false; //action activated
        }
        if (!actionBlocker){ //makes sure the action is executed only when triggered
            PlayerSwitch();    
            actionBlocker = true; // action is no longer allowed
        }
        if (!coolDownBlocker){
            coolDownTimeTemp -= Time.deltaTime;
            if (coolDownTimeTemp <= 0){ //If the cooldown is finished... 
                if (IsSwitchKeyReleased()){ // ...check if the button is still being pressed, (Without this check, as soon as the cooldown ends, if you are still holding the Switch button, you will unintentionally execute SwitchPlayer() again.)  
                    coolDownBlocker = true; // ...if not, cooldown is blocked.
                }
                coolDownTimeTemp = coolDownTime;
            }
        }
    }
    void PlayerSwitch(){
        Transform currentPlayerTransform = activePlayer.transform;
        Destroy(activePlayer);
        currentlyActivePlayer = !currentlyActivePlayer;
        PlayerSpawner();
        activePlayer.transform.position = currentPlayerTransform.position;
    }
    bool IsSwitchKeyReleased() => !inputManager.IsSwitchCharacter() ? true : false;
}
