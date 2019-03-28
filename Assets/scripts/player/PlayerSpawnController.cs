using System.Collections;
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
    private GameObject ActivePlayer;
    public bool currentlyActivePlayer = true; // t: Alice, f: Bob
    private bool actionBlocker, coolDownBlocker;

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
        ActivePlayer = PlayerInstantiator(currentlyActivePlayer ? "Alice":"Bob");
    }
    public void PlayerSwitchHandler() {
        // This method is similar to Dash(), Alice's Ability 
        // But this time, the PlayerSwitch() needs to be executed once and waits for the cooldown. 
        // We do not constantly switch again and again... while the cooldown is active.
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
            if (coolDownTimeTemp <= 0){ //iff cooldown is finished
                coolDownBlocker = true; //cooldown is blocked, and no longer gets executed
                coolDownTimeTemp = coolDownTime;
            }
        }
    }
    private void PlayerSwicth(){
        Transform currentPlayerTransform = ActivePlayer.transform;
        Destroy(ActivePlayer);
        currentlyActivePlayer = !currentlyActivePlayer;
        PlayerSpawner();
        ActivePlayer.transform.position = currentPlayerTransform.position;    
    }
    private GameObject PlayerInstantiator(string PlayerName){
        switch (PlayerName){
            case "Alice": return Instantiate(alicePrefab) as GameObject; 
            case "Bob": return Instantiate(bobPrefab) as GameObject;
            default: return null; 
        }
    }
}
