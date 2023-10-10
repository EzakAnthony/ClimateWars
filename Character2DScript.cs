using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//CHARACTER CLASS, CAN BE REUSED FOR EVERY CHARACTER
public class Character2D : MonoBehaviour
{
    //MOVEMENT VARIABLES
    public float movementSpeed = 5f;     //movement speed of player
    [Range(0, 1)] public float crouchingSpeed = 0.33f;  //crouching speed (1 is normal speed)
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    [SerializeField] private bool airControl = true;			// Whether or not a player can steer while jumping;
    [SerializeField] private float jumpForce = 100f;     // jump force
    private Vector3 userVelocity = Vector3.zero; //User velocity
    [Range(0, 1)] [SerializeField] private float airSlowDownMultiplier = 0.5f;   // by how much we slow down player when in air

    //COMPONENT VARIABLES
    private Rigidbody2D userRigidbody;    //player rigibbody
    private TrailRenderer doubleJumpRenderer;    //trailrenderer declaration
    [SerializeField] private Collider2D crouchDisableCollider; // collider that we disable when we crouch
	[SerializeField] private Transform groundCheck;		// A position marking where to check if the player is grounded.
	[SerializeField] private LayerMask whatIsGround;	// A mask determining what is ground to the character  
    [SerializeField] private Transform leftWallCheck;   // A position marking to check if player is on left wall
    [SerializeField] private LayerMask whatIsLeftWall;  // A mask determining what is the left wall to the character;
    [SerializeField] private Transform rightWallCheck;   // A position marking to check if player is on right wall
    [SerializeField] private LayerMask whatIsRightWall;  // A mask determining what is the right wall to the character;
    const float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    const float onWallRadius = 0.5f;  // Radius of the overlap circle to determine if on wall

    //DASH VARIABLES
    public float dashResetTime = 2f; // before user can redash;
    public float dashStartTime = 0f; // time at which userr initiated his dash
    public float dashForce = 100f; // dash force applied to user
    private bool canPlayerDash = true;   //check to see if the user can dash
    private float lastTimePressedDash; // when we asked for dash

    //DODGE VARIABLES
    public static bool isInvincible = false;     //determines if user is untouchable
    private float invulnerabilityTime = 0.2f; //invulnerability time
    public float dodgeStartTime = 0f; // time at which user started his dodge
    public float dodgeResetTime = 1f; //before user can redodge
    private float slowDownFactor = 2f; // by how much we slow down user when dodging
    private float lastTimePressedDodge; // when we asked for dodge
    private bool canPlayerDodge = true;

    //USER VARIABLES
    private bool facingRight;     // For determining which way the player is currently facing.
    private bool isGrounded;    //for determining if user is on the ground
    private bool isOnLeftWall;      //for determining if user is on the left wall
    private bool isOnRightWall;     //for determining if user is on the right wall
    private bool canDoubleJump;     //determines if user can double jump
    private bool isCrouching = false;   //if user is crouching
    public static float userHealth = 100f;     //user Health points

    //FOR PROJECTILE
    //private Bullet projectile;  //reference to Bullet class

    //EVENT VARIABLES
	public UnityEvent OnLandEvent;
    public UnityEvent OnLeftWallEvent;
    public UnityEvent OnRightWallEvent;
    public class BoolEvent : UnityEvent<bool> { }
	public BoolEvent OnCrouchEvent;
    
    // Start is called before the first frame update
    void Awake()
    {
        //initialize the rigid body, trail renderer and other key variables
        userRigidbody = GetComponent<Rigidbody2D>();
        //doubleJumpRenderer = GetComponent<TrailRenderer>();

        //delegate 
        if (OnLandEvent == null) {
            OnLandEvent = new UnityEvent();
        }

        // crouching event
        if (OnCrouchEvent == null) {
            OnCrouchEvent = new BoolEvent();
        }

        //wall event
        if (OnLeftWallEvent == null) {
            OnLeftWallEvent = new UnityEvent();
        }
        //wall event
        if (OnRightWallEvent == null) {
            OnRightWallEvent = new UnityEvent();
        }
    }

    //updates every fixed frame
    private void FixedUpdate() {
        //initialize was grounded to is grounded
        bool wasGrounded = isGrounded;
        isGrounded = false; //is grounded is false before we check for ground check

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] colliders =  Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i ++) {
            //if we collide with somehting different than ourselves
            if (colliders[i].gameObject != gameObject) {
                //then we are grounded
                isGrounded = true;
                if (!wasGrounded) {
                    //and tell event we are grounded
                    OnLandEvent.Invoke();
                }
            }
        }

        //intiialize wasonwall to isonwall
        bool wasOnLeftWall = isOnLeftWall;
        isOnLeftWall = false;
        // The player is on the wall if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] leftWallColliders =  Physics2D.OverlapCircleAll(leftWallCheck.position, onWallRadius, whatIsLeftWall);
        for (int i = 0; i < leftWallColliders.Length; i ++) {
            //if we collide with somehting different than ourselves
            if (leftWallColliders[i].gameObject != gameObject) {
                //then we ar eon wall
                isOnLeftWall = true;
                if (!wasOnLeftWall) {
                    //and tell event we are on wall
                    OnLeftWallEvent.Invoke();
                }
            }
        }

        //right wall check collider
        //intiialize wasonwall to isonwall
        bool wasOnRightWall = isOnRightWall;
        isOnLeftWall = false;
        // The player is on the wall if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] rightWallColliders =  Physics2D.OverlapCircleAll(rightWallCheck.position, onWallRadius, whatIsRightWall);
        for (int i = 0; i < rightWallColliders.Length; i ++) {
            //if we collide with somehting different than ourselves
            if (rightWallColliders[i].gameObject != gameObject) {
                //then we ar eon wall
                isOnRightWall = true;
                if (!wasOnRightWall) {
                    //and tell event we are on wall
                    OnRightWallEvent.Invoke();
                }
            }
        }
    }

    //moving function
    public void Move(float move, bool crouch, bool jump, int dashed, bool dodged) {

        //if we are on ground or can control ourselves in the air
        if (isGrounded || airControl) {
            //deactivate the renderer
            //doubleJumpRenderer.enabled = false;
            //if user wants to crouch
            if (crouch) {
                //if we were not crouching set isCrouchign to true
                if (!isCrouching) {
                    isCrouching = true;
                    OnCrouchEvent.Invoke(true);

                    //move at a lesser speed
                    move *= crouchingSpeed;

                    //if not initialized yet
                    if (crouchDisableCollider != null) {
                        //disable collider so that we can crouch
                        crouchDisableCollider.enabled = false;
                    }
                    // if is crouching is to true set it to false
                    if (isCrouching) {
                        isCrouching = false;
                        OnCrouchEvent.Invoke(false);
                    }
                }
            }
            //if we are in the air change the movement speed with a multiplier 
            if (!isGrounded) {
                //find the target velocity
                Vector3 targetVelocity = new Vector2(move * movementSpeed * airSlowDownMultiplier, userRigidbody.velocity.y);
                // And then smoothing it out and applying it to the character
                userRigidbody.velocity = Vector3.SmoothDamp(userRigidbody.velocity, targetVelocity, ref userVelocity, movementSmoothing);

            }
            //otherwise go to normal speed
            else {
                //find the target velocity
                Vector3 targetVelocity = new Vector2(move * movementSpeed, userRigidbody.velocity.y);
                // And then smoothing it out and applying it to the character
                userRigidbody.velocity = Vector3.SmoothDamp(userRigidbody.velocity, targetVelocity, ref userVelocity, movementSmoothing);
            }
            //if we are moving to the right but face left flip the player
            if (move < 0 && !facingRight) {
                FlipDirections();
            }
            //if we are moving left but facing right
            else if (move > 0 && facingRight) {
                FlipDirections();
            }
        }

        //if we are on ground and decide to jump 
        if (isGrounded && jump) {
            //we are not grounded anymore and we add a vertical force to the player
            isGrounded = false;
            canDoubleJump = true;
            userRigidbody.AddForce(new Vector2(0f, jumpForce));
            Debug.Log("Just Jumped");
        }
        //if we are in the air but pressed jump
        else if (!isGrounded && jump) {
            // and we have our double jump left
            if (canDoubleJump) {
                userRigidbody.AddForce(new Vector2(0f, jumpForce/2), ForceMode2D.Impulse);
                //doubleJumpRenderer.enabled = true;
                canDoubleJump = false;
            }
        }

        //if user dashed
        if (dashed != 0 && canPlayerDash) {
            //apply impulse force to user
            userRigidbody.AddForce(new Vector2(dashed * dashForce * move, 0), ForceMode2D.Impulse);
            lastTimePressedDash = Time.time; //dashed right now 
            canPlayerDash = false;
            Invoke("ResetDash", dashResetTime);
        }

        //if user dodged
        if (dodged && canPlayerDodge) { 
            //make him untouchable (0.2 seconds)
            isInvincible = true;
            lastTimePressedDodge = Time.time; //dodged now
            canPlayerDodge = false;
            Invoke("ResetDodge", dodgeResetTime);
            Invoke("ResetInvulnerability", invulnerabilityTime);
        }
        //if dodge period slow down is not over, slow down user 
        if ((Time.time - dodgeStartTime) < dodgeResetTime) {
            SlowDownMovement(userRigidbody);
        }
    }

    //reset invulnerability 
    private void ResetInvulnerability () {
        isInvincible = false;
    }

    //reset dodge variables
    private void ResetDodge() {
        canPlayerDodge = true;
    }

    //reset dash variables
    private void ResetDash() {
        canPlayerDash = true;
    }

    // Can player dodge
    private bool canDodge () {
        return (Time.time - lastTimePressedDodge > dodgeResetTime);
    }

    //check if player can dash
    private bool canDash() {
        return (Time.time - lastTimePressedDash > dashResetTime);
    }      

    //flip user facing direction
    private void FlipDirections() {
        // Switch the way the player is facing
        facingRight = !facingRight;
        transform.Rotate(0f, 180, 0);
    }

    //slow down rigidbody by a given factor
    private void SlowDownMovement(Rigidbody2D body) {
        //divide his velocity by two
        body.velocity /= slowDownFactor;
    }

    //when two objects come in collision with one another
    /*private void OnCollisionEnter2D(Collision2D collisionObject) {

        //check if it is a projectile
        if (collisionObject.gameObject.tag == "Projectile") {
            //if it is we get hit 
            //only get hit if player did not dodge and is then invincible
            if (!isInvincible) {
                //Character gets damaged and loses health points
                GetDamaged(userHealth, projectile.projectileDamage);
            }
        }
    }*/

    //Make wall jump
    public void WallJump(bool jump) {
        //if user asked to jump and is on the right wall
        if (jump && isOnRightWall && !isGrounded) {
            // add force in opposite direction of player
            userRigidbody.AddForce(new Vector2(-jumpForce, jumpForce));
        }
        //if user asked to jump and is on the left wall
        if (jump && isOnLeftWall && !isGrounded) {
            // add force in opposite direction of player
            userRigidbody.AddForce(new Vector2(jumpForce, jumpForce));
        }
    }

    //when 2d collisions happen
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = true;
        }

        // if player dodges make him untouchable for 0.2 seconds 
        //if (isInvincible) {
            //Invoke("ResetInvulnerability", 0.2f);
        //}
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

    //make player take damage
    /*public void GetDamaged(float healthCharacter, float damage) {
        healthCharacter = healthCharacter - damage;
        //If no more health
        if (healthCharacter <= 0) {
            //we die or lose a life or whatever
            LoseLife();
        }
    }

    //Destroy gameobject if no more life
    private void LoseLife() {
        Destroy(gameObject);
    }*/

}
