using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using Photon.Pun;

//SHOULD MIMICK A CAN
public class CanWeapon : MonoBehaviour
{
    //Projectile settings
    private float projectileDamage = 25f;    //can damage
    public static float canSpeed = 10f; //initial can speed if not charged (TBC)
    private float gravityEffect = 1f;    //gravoity effect on can (try to mimic a true spear)
    //private int numberTotalLives = 3;   //number of total  lives before game is over
    //private PhotonView PV;  //reference to photon view

    //initialize its properties
    private void Start() {
        //gravity and mass
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityEffect;
        //rb.mass = 100;
        //PV = GetComponent<PhotonView>();
    }

    //when the can hits something, will take damage here afterwards
    private void OnCollisionEnter2D(Collision2D collisionObject) {
        //Debug.Log("JUST HIT" + collisionObject.gameObject.name);
        //if we hit a player and he is not invicible (just dodged)
        /*
        if (collisionObject.gameObject.tag.Equals("Player") && !Character2DScript.isInvincible) {
            //deal damage to player accross network
            PV.RPC("RPCDealDamageToPlayer", RpcTarget.All);
            
            if (Character2DScript.userHealth <= 0) {
                PV.RPC("RPCKillPlayer", RpcTarget.All, collisionObject.gameObject);
                //RPCKillPlayer(collisionObject.gameObject);
            }
        }*/
    }

    /*void OnTriggerEnter2D(Collider2D hitBulletInfo) {
        Debug.Log(hitBulletInfo.name);
        //gather component
        Character2DScript player = hitBulletInfo.GetComponent<Character2DScript>();
        //if player is hit and did not just dodge
        if (player != null && !player.isInvincible) {
            //take damage 
            //player.GetDamaged(player.userHealth, projectileDamage);
        }
    }*/

    private void FixedUpdate() {
        //gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX ;  
        //gameObject.GetComponent<Rigidbody2D>();
    }

    //this video takes away some of its health corresponding to the projectile damage (here the can)
    //[PunRPC]
    private void DealDamageToPlayer() {
        //Character2DScript.userHealth -= projectileDamage;
    }

    //kill player and respawn him he has lives remaining 
    //[PunRPC]
    /*private IEnumerable RPCKillPlayer(GameObject gameObj) {        
        //disable him for 5 seconds and then we make him respawn
        //gameObj.active = false; 
        //take away one of his life
        numberTotalLives--;
        //if we are out of lives destroy gameObject
        if (numberTotalLives <= 0) {
            Destroy(gameObj);
            Debug.Log("GAME OVER!");
        }
        else {
            yield return new WaitForSeconds(5);
            //place player on spawn point depending on team
            //TODO: CHANGE ACCORDING TO PREVIOUS TEAM
            gameObj.transform.position = new Vector3(GameSetup.gameSetup.spawnPointsTeam1[0].position.x, GameSetup.gameSetup.spawnPointsTeam1[0].position.y, GameSetup.gameSetup.spawnPointsTeam1[0].position.z);
        }
    }*/

    public GameObject gameObject;
    //PickUpWeapons pickUpWeapons = new PickUpWeapons();
    //no knockback force on hanger
    private void OnTriggerEnter2D (Collider2D collisionObject) {
        //Debug.Log("JUST HIT" + other.gameObject.name);
        //if (collisionObject.gameObject.tag.Equals("Player") && !Character2DScript.isInvincible) {
        //if we hit a player and he is not invicible (just dodged)
        if (collisionObject.gameObject.tag == "Player") {
            //search for all available weapons
            //for (int i = 0; i < PickUpWeapons.weaponsToPickUp.Count; i++) {
                //if the exact name of the weapon is the following, we pick it up and make it available for throwing
                //if (other.gameObject.name == "Can" + i.ToString() + "(Clone)") {
                    //Debug.Log("GO" + gameObject);
                    
            collisionObject.gameObject.GetComponent<PickUpWeapons>().PickUp(gameObject);
            //pickUpWeapons.PickUp(gameObject);
            //if (PV.IsMine) {
            //    PV.RPC("PickUp", RpcTarget.AllViaServer, i);
            //}
            
            PickUpWeapons.weaponToThrow = gameObject;
            //can now throw the object
            PickUpWeapons.canThrow = true;
        }
        if (collisionObject.gameObject.tag == "Ground") {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f; //disable gravity on the objected collided into (IE, this one)
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;; //disable velocity on the objected collided into (IE, this one)
            gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0f;; //disable velocity on the objected collided into (IE, this one)
            //Debug.Log("Disabled Gravity");
        }
    }
    private void OnTriggerStay2D(Collider2D collisionObject) {
        //Debug.Log("JUST HIT" + other.gameObject.name);
        //if (collisionObject.gameObject.tag.Equals("Player") && !Character2DScript.isInvincible) {
        //if we hit a player and he is not invicible (just dodged)
        if (collisionObject.gameObject.tag == "Player") {
            //search for all available weapons
            //for (int i = 0; i < PickUpWeapons.weaponsToPickUp.Count; i++) {
                //if the exact name of the weapon is the following, we pick it up and make it available for throwing
                //if (other.gameObject.name == "Can" + i.ToString() + "(Clone)") {
                    //pickUpWeapons.PickUp(gameObject);
                    
            collisionObject.gameObject.GetComponent<PickUpWeapons>().PickUp(gameObject);
            //if (PV.IsMine) {
            //    PV.RPC("PickUp", RpcTarget.AllViaServer, i);
            //}
            
            PickUpWeapons.weaponToThrow = gameObject;
            //can now throw the object
            PickUpWeapons.canThrow = true;
        }
    }
}
