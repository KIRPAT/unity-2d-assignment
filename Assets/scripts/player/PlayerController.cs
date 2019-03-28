using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerController : MonoBehaviour {

    protected inputManager inputManager; 
    [HideInInspector] private bool left, right, jump, switchCharacter;
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
        inputManager = GetComponentInParent<inputManager>();
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
       if (!isSpecialMoveTriggered) HorizontalMovement();
    }
    
   

    private void HorizontalMovement(){ 
        if (inputManager.IsRight()){
            playerRigidBody.velocity = new Vector2(horizontalSpeed * 1, playerRigidBody.velocity.y);
            currentCharacterDirection = true;
        } else if (inputManager.IsLeft()){
            playerRigidBody.velocity = new Vector2(-horizontalSpeed * 1, playerRigidBody.velocity.y);
            currentCharacterDirection = false;
        } else if (!inputManager.IsRight() && !inputManager.IsLeft()) {
            playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);
        }  
        characterSpriteRenderer.flipX = !currentCharacterDirection;
    }
    private void JumpHandler(){ 
        if(inputManager.IsJump() && isGrounded && !jumpActionBlocker){
            Debug.Log("Jump Press");
            playerRigidBody.velocity = Vector2.up * jumpForce;
        }
        jumpActionBlocker = IsJumpButtonOnRelease() ? false : true; //Prevents unintentional multiple jumps from Collision Detection latency. 
    }
    private void IsGroundedSetter(){ // Collision Detector
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    protected bool IsJumpButtonOnRelease() => !inputManager.IsJump() ? true : false;
    
    //ABSTRACT METHODS
    abstract public void SpecialMovePropertyInitializer();
    abstract public void SpecialMoveTrigger();
    abstract public void SpecialMoveHandler();   
}
