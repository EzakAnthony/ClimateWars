using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;

public class PickUpWeapons : MonoBehaviour
{   
    //PickUp weapons class
    public static bool canHold = true; //can hold weapon
    //public Transform guide;    //reference to player
    [SerializeField]public Transform guide;    //reference to player
    public static List<GameObject> weaponsToPickUp = new List<GameObject>();     //list of weapons spawned over the course of the game
    private bool pressedPickUp; //check command to pick up
    private bool isWeapon = false;    //check if we are near the weapon
    public static bool canThrow = false;       //boolean that decides if we can throw the object
    public static GameObject weaponToThrow;    //weapon integer that designs which weapon we throw in weapon.cs
    //[SerializeField] private PhotonView PV;  //photon view componennt

    private void Start() {
        //PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        //if user presses on pickup button
        if (Input.GetButtonDown("PickUp")) {
            pressedPickUp = true;

            //Debug.Log("Pressed picked Up " + pressedPickUp);
        }
        //if user presses on pickup button
        if (Input.GetButtonUp("PickUp")) {
            pressedPickUp = false;
        }
    }

    public void PickUp(GameObject gameObject) {

        //Debug.Log("Pressed picked Up " + pressedPickUp);
        if (canHold && pressedPickUp){
            //Debug.Log("Picked Up " + gameObject.tag);
            //make object belong to the transform and set its position at the transform
            gameObject.transform.parent = guide;
            gameObject.transform.position = guide.position;
            //disable its collider
            gameObject.GetComponent<Collider2D>().enabled = false;
            //set velocity to 0 so that when we put it as kinematic we dont fly with the weapon
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
            //disables its rigidbody  so that it stays within our hand
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            //can not hold two object
            canHold = false;
            //Debug.Log("Picked Up " + gameObject.tag);
        }
    }
    
    /*private void OnTriggerEnter2D (Collider2D collisionObject) {
        if(collisionObject.gameObject.tag == "Can" || collisionObject.gameObject.tag == "GlassBottle" || 
        collisionObject.gameObject.tag == "Boomerang" || collisionObject.gameObject.tag == "Straw" || collisionObject.gameObject.tag == "Bullet" ||
        collisionObject.gameObject.tag == "SpikedBall")  {
            isWeapon = true;
        }

        Debug.Log("On Collision" + collisionObject.gameObject.tag + isWeapon);
        //nearObject = true;
        //if we can hold object, asked to pick it up and in its vincinity
        //MUST CHECK IF ENEMY BULLET OR NOT BECAUSE THIS SHOULD NOT BE POSSIBLE OTHERWISE (OR COULD IT)?
        if(canHold && pressedPickUp && isWeapon) {
        //if(canHold && pressedPickUp && nearObject && isWeapon) {

            Debug.Log("On Collision" + collisionObject.gameObject.tag + isWeapon);
            //if the object we are in collision with contains the following name
            if (collisionObject.gameObject.tag == "Can") {
                //search for all available weapons
                for (int i = 0; i < weaponsToPickUp.Count; i++) {
                    //if the exact name of the weapon is the following, we pick it up and make it available for throwing
                    if (collisionObject.gameObject.name == "Can" + i.ToString() + "(Clone)") {
                        PickUp(i);
                        //if (PV.IsMine) {
                        //    PV.RPC("PickUp", RpcTarget.AllViaServer, i);
                        //}
                        
                        //weaponToThrow = i;
                        //can now throw the object
                        canThrow = true;
                    }
                }
            }
            else if (collisionObject.gameObject.tag == "Boomerang") {
                for (int i = 0; i < weaponsToPickUp.Count; i++) {
                    if (collisionObject.gameObject.name == "Boomerang" + i.ToString() + "(Clone)") {
                        PickUp(i);
                        //PV.RPC("PickUp", RpcTarget.AllViaServer, i);
                        //weaponToThrow = i;
                        //can now throw the object
                        canThrow = true;
                    }
                }
            }
            else if (collisionObject.gameObject.tag == "GlassBottle") {
                for (int i = 0; i < weaponsToPickUp.Count; i++) {
                    if (collisionObject.gameObject.name == "GlassBottle" + i.ToString() + "(Clone)") {
                        PickUp(i);
                        //PV.RPC("PickUp", RpcTarget.AllViaServer, i);
                        //weaponToThrow = i;
                        //can now throw the object
                        canThrow = true;
                    }
                }
            }
            else if (collisionObject.gameObject.tag == "Straw") {
                for (int i = 0; i < weaponsToPickUp.Count; i++) {
                    if (collisionObject.gameObject.name == "Straw" + i.ToString() + "(Clone)") {
                        PickUp(i);
                        //PV.RPC("PickUp", RpcTarget.AllViaServer, i);
                        //weaponToThrow = i;
                        //can now throw the object
                        canThrow = true;
                    }
                }
            }
            else if (collisionObject.gameObject.tag == "SpikedBall") {
                for (int i = 0; i < weaponsToPickUp.Count; i++) {
                    if (collisionObject.gameObject.name == "SpikedBall" + i.ToString() + "(Clone)") {
                        PickUp(i);
                        //PV.RPC("PickUp", RpcTarget.AllViaServer, i);
                        //weaponToThrow = i;
                        //can now throw the object
                        canThrow = true;
                    }
                }
            }
            else if (collisionObject.gameObject.tag == "Bullet") {
                for (int i = 0; i < weaponsToPickUp.Count; i++) {
                    if (collisionObject.gameObject.name == "Bullet" + i.ToString() + "(Clone)") {
                        PickUp(i);
                        //PV.RPC("PickUp", RpcTarget.AllViaServer, i);
                        //weaponToThrow = i;
                        //can now throw the object
                        canThrow = true;
                    }
                }
            }
            //put user input to false
            pressedPickUp = false;

            Debug.Log("Can throw: " + canThrow);
        }
    }
*/
    /*
    //staying in collision with object
    private void OnCollisionStay2D(Collision2D collisionObject) {
        //if (PV.IsMine) {
        //collision with weapon
        if(collisionObject.gameObject.tag == "Can" || collisionObject.gameObject.tag == "GlassBottle" || 
        collisionObject.gameObject.tag == "Boomerang" || collisionObject.gameObject.tag == "Straw" || collisionObject.gameObject.tag == "Bullet" ||
        collisionObject.gameObject.tag == "SpikedBall")  {
            isWeapon = true;
        }
        //nearObject = true;
        //if we can hold object, asked to pick it up and in its vincinity
        //MUST CHECK IF ENEMY BULLET OR NOT BECAUSE THIS SHOULD NOT BE POSSIBLE OTHERWISE (OR COULD IT)?
        if(canHold && pressedPickUp && isWeapon) {
        //if(canHold && pressedPickUp && nearObject && isWeapon) {

            Debug.Log("On Collision" + collisionObject.gameObject.tag + isWeapon);
            //if the object we are in collision with contains the following name
            if (collisionObject.gameObject.tag == "Can") {
                //search for all available weapons
                for (int i = 0; i < weaponsToPickUp.Count; i++) {
                    //if the exact name of the weapon is the following, we pick it up and make it available for throwing
                    if (collisionObject.gameObject.name == "Can" + i.ToString() + "(Clone)") {
                        PickUp(i);
                        //if (PV.IsMine) {
                        //    PV.RPC("PickUp", RpcTarget.AllViaServer, i);
                        //}
                        
                        weaponToThrow = i;
                        //can now throw the object
                        canThrow = true;
                    }
                }
            }
            else if (collisionObject.gameObject.tag == "Boomerang") {
                for (int i = 0; i < weaponsToPickUp.Count; i++) {
                    if (collisionObject.gameObject.name == "Boomerang" + i.ToString() + "(Clone)") {
                        PickUp(i);
                        //PV.RPC("PickUp", RpcTarget.AllViaServer, i);
                        weaponToThrow = i;
                        //can now throw the object
                        canThrow = true;
                    }
                }
            }
            else if (collisionObject.gameObject.tag == "GlassBottle") {
                for (int i = 0; i < weaponsToPickUp.Count; i++) {
                    if (collisionObject.gameObject.name == "GlassBottle" + i.ToString() + "(Clone)") {
                        PickUp(i);
                        //PV.RPC("PickUp", RpcTarget.AllViaServer, i);
                        weaponToThrow = i;
                        //can now throw the object
                        canThrow = true;
                    }
                }
            }
            else if (collisionObject.gameObject.tag == "Straw") {
                for (int i = 0; i < weaponsToPickUp.Count; i++) {
                    if (collisionObject.gameObject.name == "Straw" + i.ToString() + "(Clone)") {
                        PickUp(i);
                        //PV.RPC("PickUp", RpcTarget.AllViaServer, i);
                        weaponToThrow = i;
                        //can now throw the object
                        canThrow = true;
                    }
                }
            }
            else if (collisionObject.gameObject.tag == "SpikedBall") {
                for (int i = 0; i < weaponsToPickUp.Count; i++) {
                    if (collisionObject.gameObject.name == "SpikedBall" + i.ToString() + "(Clone)") {
                        PickUp(i);
                        //PV.RPC("PickUp", RpcTarget.AllViaServer, i);
                        weaponToThrow = i;
                        //can now throw the object
                        canThrow = true;
                    }
                }
            }
            else if (collisionObject.gameObject.tag == "Bullet") {
                for (int i = 0; i < weaponsToPickUp.Count; i++) {
                    if (collisionObject.gameObject.name == "Bullet" + i.ToString() + "(Clone)") {
                        PickUp(i);
                        //PV.RPC("PickUp", RpcTarget.AllViaServer, i);
                        weaponToThrow = i;
                        //can now throw the object
                        canThrow = true;
                    }
                }
            }
            //put user input to false
            pressedPickUp = false;

            Debug.Log("Can throw: " + canThrow);
        }
    }

    //enter near object    
    private void OnCollisionEnter2D(Collision2D collisionObject) {
        //collision with weapon
        if(collisionObject.gameObject.tag == "Can" || collisionObject.gameObject.tag == "GlassBottle" || 
        collisionObject.gameObject.tag == "Boomerang" || collisionObject.gameObject.tag == "Straw" || collisionObject.gameObject.tag == "Bullet" ||
        collisionObject.gameObject.tag == "SpikedBall")  {
            isWeapon = true;
        }
        //nearObject = true;
        //if we can hold object, asked to pick it up and in its vincinity
        //MUST CHECK IF ENEMY BULLET OR NOT BECAUSE THIS SHOULD NOT BE POSSIBLE OTHERWISE (OR COULD IT)?
        //if(canHold && pressedPickUp && nearObject && isWeapon) {
        if(canHold && pressedPickUp && isWeapon) {
            //if the object we are in collision with contains the following name
            if (collisionObject.gameObject.tag == "Can") {
                //search for all available weapons
                for (int i = 0; i < weaponsToPickUp.Count; i++) {
                    //if the exact name of the weapon is the following, we pick it up and make it available for throwing
                    if (collisionObject.gameObject.name == "Can" + i.ToString() + "(Clone)") {
                        PickUp(i);
                        //if (PV.IsMine) {
                        //    PV.RPC("PickUp", RpcTarget.AllViaServer, i);
                        //}
                        
                        weaponToThrow = i;
                        //can now throw the object
                        canThrow = true;
                    }
                }
            }
            else if (collisionObject.gameObject.tag == "Boomerang") {
                for (int i = 0; i < weaponsToPickUp.Count; i++) {
                    if (collisionObject.gameObject.name == "Boomerang" + i.ToString() + "(Clone)") {
                        PickUp(i);
                        //PV.RPC("PickUp", RpcTarget.AllViaServer, i);
                        weaponToThrow = i;
                        //can now throw the object
                        canThrow = true;
                    }
                }
            }
            else if (collisionObject.gameObject.tag == "GlassBottle") {
                for (int i = 0; i < weaponsToPickUp.Count; i++) {
                    if (collisionObject.gameObject.name == "GlassBottle" + i.ToString() + "(Clone)") {
                        PickUp(i);
                        //PV.RPC("PickUp", RpcTarget.AllViaServer, i);
                        weaponToThrow = i;
                        //can now throw the object
                        canThrow = true;
                    }
                }
            }
            else if (collisionObject.gameObject.tag == "Straw") {
                for (int i = 0; i < weaponsToPickUp.Count; i++) {
                    if (collisionObject.gameObject.name == "Straw" + i.ToString() + "(Clone)") {
                        PickUp(i);
                        //PV.RPC("PickUp", RpcTarget.AllViaServer, i);
                        weaponToThrow = i;
                        //can now throw the object
                        canThrow = true;
                    }
                }
            }
            else if (collisionObject.gameObject.tag == "SpikedBall") {
                for (int i = 0; i < weaponsToPickUp.Count; i++) {
                    if (collisionObject.gameObject.name == "SpikedBall" + i.ToString() + "(Clone)") {
                        PickUp(i);
                        //PV.RPC("PickUp", RpcTarget.AllViaServer, i);
                        weaponToThrow = i;
                        //can now throw the object
                        canThrow = true;
                    }
                }
            }
            else if (collisionObject.gameObject.tag == "Bullet") {
                for (int i = 0; i < weaponsToPickUp.Count; i++) {
                    if (collisionObject.gameObject.name == "Bullet" + i.ToString() + "(Clone)") {
                        PickUp(i);
                        //PV.RPC("PickUp", RpcTarget.AllViaServer, i);
                        weaponToThrow = i;
                        //can now throw the object
                        canThrow = true;
                    }
                }
            }
            //put user input to false
            pressedPickUp = false;
        }

        Debug.Log("Can throw: " + canThrow);
    }*/

    //if we exit out out of collision with object
    /*private void OnCollisionExit2D(Collision2D collisionObject) {
        //if (PV.IsMine) {
            //if we exit of collision with object
        if(collisionObject.gameObject.tag == "Can" || collisionObject.gameObject.tag == "GlassBottle" || 
        collisionObject.gameObject.tag == "Boomerang" || collisionObject.gameObject.tag == "Straw" || collisionObject.gameObject.tag == "Bullet" ||
        collisionObject.gameObject.tag == "SpikedBall")   {
            //if(collisionObject.gameObject.tag == "Weapon") {
                //we are not within object range anymore
                //nearObject = false;
                isWeapon = false;
            }
        //}
    }*/

    //if we exit out out of trigger with object
    /*private void OnTriggerExit2D(Collider2D collisionObject) {
        //if (PV.IsMine) {
            //if we exit of collision with object
        if(collisionObject.gameObject.tag == "Can" || collisionObject.gameObject.tag == "GlassBottle" || 
        collisionObject.gameObject.tag == "Boomerang" || collisionObject.gameObject.tag == "Straw" || collisionObject.gameObject.tag == "Bullet" ||
        collisionObject.gameObject.tag == "SpikedBall")   {
            //if(collisionObject.gameObject.tag == "Weapon") {
                //we are not within object range anymore
                //nearObject = false;
                isWeapon = false;
            }
        //}
    }*/

    //pick up weapon on ground
    //[PunRPC]
    /*public void PickUp(int indexWeapon) {
        //make object belong to the transform and set its position at the transform
        weaponsToPickUp[indexWeapon].transform.parent = guide;
        weaponsToPickUp[indexWeapon].transform.position = guide.position;
        //disable its collider
        weaponsToPickUp[indexWeapon].GetComponent<Collider2D>().enabled = false;
        //set velocity to 0 so that when we put it as kinematic we dont fly with the weapon
        weaponsToPickUp[indexWeapon].GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        //disables its rigidbody  so that it stays within our hand
        weaponsToPickUp[indexWeapon].GetComponent<Rigidbody2D>().isKinematic = true;
        //can not hold two object
        canHold = false;
        Debug.Log("Picked Up " + weaponsToPickUp[indexWeapon].gameObject.tag);
    }*/
}
