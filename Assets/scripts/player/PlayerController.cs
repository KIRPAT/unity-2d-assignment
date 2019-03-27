using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerController : MonoBehaviour {
    //HorizontalCharacterMovement
    public float horizontalSpeed;
    public Rigidbody2D playerRigidBody;
    
    //Jump 
    public float jumpForce;

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
        if (Input.GetKey(KeyCode.D)){
            playerRigidBody.velocity = new Vector2(horizontalSpeed * 1, playerRigidBody.velocity.y);
            currentCharacterDirection = true;
        } else if (Input.GetKey(KeyCode.A)){
            playerRigidBody.velocity = new Vector2(-horizontalSpeed * 1, playerRigidBody.velocity.y);
            currentCharacterDirection = false;
        } else {
            playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);
        }  
        characterSpriteRenderer.flipX = !currentCharacterDirection;
    }
    private void JumpHandler(){ 
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            Debug.Log("Jump Press");
            playerRigidBody.velocity = Vector2.up * jumpForce;
        }
    }
    private void IsGroundedSetter(){ // Collision Detector
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }
    
    //ABSTRACT METHODS
    abstract public void SpecialMovePropertyInitializer();
    abstract public void SpecialMoveTrigger();
    abstract public void SpecialMoveHandler();   
}
