using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //MOVEMENT VARIABLES
    public float movementSpeed = 5f;     //movement speed of player
    [Range(0, 1)] public float crouchingSpeed = 0.33f;  //crouching speed
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    public float jumpForce = 10f;     // jump force
    private Vector3 userVelocity = Vector3.zero; //User velocity

    //COMPONENT VARIABLES
    private Rigidbody2D userRigidbody;    //player rigibbody
    //private TrailRenderer doubleJumpRenderer;    //trailrenderer declaration
    [SerializeField] private Collider2D crouchDisableCollider; // collider that we disable when we crouch

    //KEYS PRESSED VARIABLES
    private bool rightArrowPressed = false;
    private bool leftArrowPressed = false;
    private bool spaceBarPressed = false;
    private bool eKeyPressed = false;

    //DASH VARIABLES
    float lastTimePressedDash; //key for dash 
    bool canPlayerDash; // boolean that holds results of can dash method
    private float dashResetTime = 2f; // before user can redash;
    private bool didDashRight = false; // check if user dashed to the right
    private bool didDashLeft = false;  // check if the user dashed to the left 
    private float dashStartTime; // time at which userr initiated his dash
    public float dashForce = 100f; // dash force applied to user

    //DODGE VARIABLES
    private bool isInvincible = false;     //determines if user is untouchable
    float lastTimePressedDodge;//key for invulnerability
    bool canPlayerDodge;  //boolean that holds results of can dodge method
    private float invulnerabilityTime; //invulnerability time
    private float dodgeStartTime; // time at which user started his dodge
    private float dodgeResetTime = 1f; //before user can redodge
    private bool didDodge = false; // if user dodge 

    //MISCEALLENOUS VARIABLES
    private bool facingRight;     // For determining which way the player is currently facing.
    private bool isGrounded;    //for determining if user is on the ground
    private bool canDoubleJump = true;     //determines if user can double jump

    //check if player can dodge
    private bool canDodge () {
        return (Time.time - lastTimePressedDodge > dodgeResetTime);
    }

    //check if player can dash
    private bool canDash() {
        return (Time.time - lastTimePressedDash > dashResetTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        //initialize the rigid body, trail renderer and other key variables
        userRigidbody = GetComponent<Rigidbody2D>();
        //doubleJumpRenderer = GetComponent<TrailRenderer>();

        //Variable initialization
        //isGrounded = true;
        facingRight = true;
        //dodge
        lastTimePressedDodge = Time.time - dodgeResetTime;
        canPlayerDodge = true;
        //dash
        lastTimePressedDash = Time.time - dashResetTime;
        canPlayerDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        //check for key inputs 
        if (Input.GetKey(KeyCode.RightArrow)) {
            rightArrowPressed = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            leftArrowPressed = true;
        }
        if (Input.GetKey(KeyCode.Space)) {
            spaceBarPressed = true;
        }
        //check for key that enables dodge mechanic
        canPlayerDodge = canDodge();
        if (Input.GetKey(KeyCode.E) && canPlayerDodge) {
            lastTimePressedDodge = Time.time;
            didDodge = true;
            canPlayerDodge = false;
        }

        //check for key that enables dash mechanic
        canPlayerDash = canDash();
        if (Input.GetKey(KeyCode.R) && canPlayerDash) {
            lastTimePressedDash = Time.time;
            didDashRight = true;
            canPlayerDash = false;
            Debug.Log("DASHED");
        }
        /*else if (Input.GetKey(KeyCode.R) && canPlayerDash) {
            lastTimePressedDash = Time.time;
            didDashLeft = true;
            canPlayerDash = false;
        }*/
    }
    
    //update every fixed frame
    void FixedUpdate() {

        //if player is on groundd he can double jump and disable the trail renderer that appears on jump #2
        if (isGrounded) {
            canDoubleJump = true;
            //doubleJumpRenderer.enabled = false;
        }

        //if he pressed right
        if (rightArrowPressed == true) {
            if (didDodge) {
                userRigidbody.velocity = new Vector2(movementSpeed/2, 0);
                //userRigidbody.AddForce(new Vector2(movementSpeed/2, 0), ForceMode2D.Impulse);
                Debug.Log("Movement Reduced");
                rightArrowPressed = false;
            }
            //if user dashed right
            else if (didDashRight) {
                userRigidbody.velocity = new Vector2(movementSpeed, 0);
                //add a force to dash quickly 
                userRigidbody.AddForce(new Vector2(dashForce, 0), ForceMode2D.Impulse);
                didDashRight = false;
                //userRigidbody.AddForce(new Vector2(movementSpeed * 2f, 0), ForceMode2D.Impulse);
                rightArrowPressed = false;
            } 
            //if user dashed left
            else if (didDashLeft) {
                userRigidbody.velocity = new Vector2(movementSpeed, 0);
                //add a force to dash quickly 
                userRigidbody.AddForce(new Vector2(-dashForce, 0), ForceMode2D.Impulse);
                didDashLeft = false;
                rightArrowPressed = false;
            }
            else {
                userRigidbody.velocity = new Vector2(movementSpeed, 0);
                //userRigidbody.AddForce(new Vector2(movementSpeed, 0), ForceMode2D.Impulse);
                rightArrowPressed = false;
            }
        }
        //if he pressed left
        if (leftArrowPressed == true) {
            //if user dodged
            if (didDodge) {
                userRigidbody.velocity = new Vector2(-movementSpeed/2, 0);
                Debug.Log("Movement Reduced");
                leftArrowPressed = false;
            }
            //if user dashed left
            else if (didDashLeft) {
                userRigidbody.velocity = new Vector2(-movementSpeed, 0);
                //add a force to dash quickly 
                userRigidbody.AddForce(new Vector2(-dashForce, 0), ForceMode2D.Impulse);
                didDashLeft = false;
                leftArrowPressed = false;
            }
            //if user dashed right
            else if (didDashRight) {
                userRigidbody.velocity = new Vector2(-movementSpeed, 0);
                //add a force to dash quickly 
                userRigidbody.AddForce(new Vector2(dashForce, 0), ForceMode2D.Impulse);
                didDashRight = false;
                //userRigidbody.AddForce(new Vector2(movementSpeed * 2f, 0), ForceMode2D.Impulse);
                leftArrowPressed = false;
            } 
            //if user just moved without extra movement
            else {
                userRigidbody.velocity = new Vector2(-movementSpeed, 0);
                leftArrowPressed = false;
            }
        }
        //if he pressed space bar
        if (spaceBarPressed == true) {
            if (isGrounded) {
                userRigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            } else {
                if (Input.GetKey(KeyCode.Space)) {
                    if (canDoubleJump) {
                        //doubleJumpRenderer.enabled = true;
                        userRigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                        canDoubleJump = false;
                    }
                }
            }
            spaceBarPressed = false;
        }

        // if player dodges and is within the invulnerability window
        if (isInvincible) {
            Invoke("ResetInvulnerability", invulnerabilityTime);
        }
        // if the user dodged
        if (didDodge) {
            Invoke("ResetDodge", dodgeResetTime);
        }
        //if the user dashed
        if (didDashRight || didDashLeft) {
            Invoke("ResetDash", dashResetTime);
        }
    }


    //when 2d collisions happen
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = true;
        }

        // if player dodges make him untouchable for 0.2 seconds 
        if (isInvincible) {
            Invoke("ResetInvulnerability", 0.2f);
        }
        //if he did not dodge, get hit 
        else {
        }

    }

    //when exit out of collision box
    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = false;
        }
    }

    //flip user facing direction
    private void FlipDirections() {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        transform.Rotate(0f, 180, 0);
    }

    //control when in air
    private void MoveMidAir() {
        float midAirSpeed = 0.5f;
        if (Input.GetKey(KeyCode.RightArrow)) {
            userRigidbody.AddForce(new Vector2(midAirSpeed, 0), ForceMode2D.Impulse);
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            userRigidbody.AddForce(new Vector2(-midAirSpeed, 0), ForceMode2D.Impulse);
        }
    }

    //reset invulnerability 
    private void ResetInvulnerability () {
        isInvincible = false;
    }

    //reset dodge variables
    private void ResetDodge() {
        didDodge = false;
        canPlayerDodge = true;
    }

    //reset dash variables
    private void ResetDash() {
        didDashRight = false;
        didDashLeft = false;
        canPlayerDash = true;
    }
}
