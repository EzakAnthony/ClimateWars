using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMvmt : MonoBehaviour
{

    //KEYS PRESSED VARIABLES
    private bool jump = false;
    private bool dodge = false;
    private float horizontalMovement = 0f;
    private bool crouch = false;
    private int dash = 0;

    //PLAYER
    public Character2D player;
    //public CharacterController2D player;

    // Update is called once per frame
    void Update()
    {
        //check for key inputs         
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Crouch")) {
            crouch = true;
        }
        if (Input.GetButtonDown("Jump")) {
            jump = true;
        }
        if (Input.GetButtonDown("Dodge")) {
            dodge = true;
        }   
        if (Input.GetButtonDown("Dash")) {
            dash = 1;
        }
    }
    
    //update every fixed frame
    void FixedUpdate() {
        player.Move(horizontalMovement, crouch, jump, dash, dodge);
        jump = false;
        dash = 0;
        dodge = false;
        crouch = false;
        //player.Move(horizontalMovement, crouch, jump);
    }

}
