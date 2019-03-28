using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerController : MonoBehaviour {
    //HorizontalCharacterMovement
    public float horizontalSpeed;
    public Rigidbody2D playerRigidBody;
    
    //Jump 
    public float jumpForce;
    protected bool jumpActionBlocker;

    //CharacterGroundHitboxCheckers
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    //SpecialMove
    protected bool isSpecialMoveTriggered; 

    //Sprite
    public SpriteRenderer characterSpriteRenderer;
    public bool isCharacterLookingLeft = false;
    protected bool currentCharacterDirection;  //true: right, false: left
    
    //MONO-BEHAVIOUR METHODS
    void Start(){
        //Local Properties
        jumpActionBlocker = false;
        isSpecialMoveTriggered = false;
        currentCharacterDirection = isCharacterLookingLeft ? false : true; //Determines starting direction for the character 
        //Abstract Properties
        SpecialMovePropertyInitializer();
    } 
    void Update(){
        //Physics State Setters
        IsGroundedSetter();
        //Abstract Methods
        SpecialMoveTrigger(); 
        SpecialMoveHandler(); 
        //Local Handler Methods
        HorizontalMovementHandler();
        JumpHandler();
    }

    //SHARED METHODS
    private void HorizontalMovementHandler(){ 
        if (!isSpecialMoveTriggered){
            KeyboardControls();
        }
    }

    private void KeyboardControls(){ 
        if (Input.GetAxis("Horizontal") > 0){
            playerRigidBody.velocity = new Vector2(horizontalSpeed * 1, playerRigidBody.velocity.y);
            currentCharacterDirection = true;
        } else if (Input.GetAxis("Horizontal") < 0){
            playerRigidBody.velocity = new Vector2(-horizontalSpeed * 1, playerRigidBody.velocity.y);
            currentCharacterDirection = false;
        } else {
            playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);
        }  
        characterSpriteRenderer.flipX = !currentCharacterDirection;
    }
    private void JumpHandler(){ 
        if(Input.GetAxis("Jump") > 0 && isGrounded && !jumpActionBlocker){
            Debug.Log("Jump Press");
            playerRigidBody.velocity = Vector2.up * jumpForce;
        }
        jumpActionBlocker = IsButtonOnRelease("Jump") ? false : true; //Prevents unintentional multiple jumps from Collision Detection latency. 
    }
    private void IsGroundedSetter(){ // Collision Detector
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    protected bool IsButtonOnRelease(string buttonName) => Input.GetAxis(buttonName) == 0 ? true : false;
    
    //ABSTRACT METHODS
    abstract public void SpecialMovePropertyInitializer();
    abstract public void SpecialMoveTrigger();
    abstract public void SpecialMoveHandler();   
}
