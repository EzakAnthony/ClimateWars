

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//SHOULD MIMICK A MOLOTOV COCKTAIL


public class GlassBottle : MonoBehaviour
{

    public static float bottleSpeed = 10f; //initial glass bottle speed if not charged (TBC)
    private float gravityEffect = 1f;    //gravoity effect on bottle (try to mimic a true spear)
    public float bottleDamage = 25f; //should correspond to speed and hitbox
    public float explosionDamage = 15f;// how much the blast does as extra damage
    public float explosionForce = 30f; // force of explosion upon impact

    //initialize its properties
    private void Start() {
        //gravity and mass
        //Rigidbody2D rb = GetComponent<Rigidbody2D>();
        //rb.gravityScale = gravityEffect;
        //rb.mass = 100;
        //PV = GetComponent<PhotonView>();
    }

     //when the can hits something, will take damage here afterwards
    private void OnCollisionEnter2D(Collision2D collisionObject) {
        //Debug.Log("JUST HIT" + collisionObject.gameObject.name);
        //if hit player then take damage 
        //explode
        //Explode(rb, explosionForce, collisionObject.transform, 10);

        //draw a circle around the bottle that damages the collisions
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, k_GroundedRadius);
        //check for all colliders
		//for (int i = 0; i < colliders.Length; i++)
		//{
            //if a collision happens with something other than itself
			//if (colliders[i].gameObject != gameObject)
			//{
                //if the object is the player and did not dodge, make him take damage
                //if (collisionObject.gameObject.tag.Equals("Player") && !Character2DScript.isInvincible) {
                //}
            //}
        //}
        //if we hit a player and he is not invicible (just dodged)
        
        /*if (collisionObject.gameObject.tag.Equals("Player") && !Character2DScript.isInvincible) {
            //deal damage to player accross network
            PV.RPC("RPCDealDamageToPlayer", RpcTarget.All);
            
            if (Character2DScript.userHealth <= 0) {
                PV.RPC("RPCKillPlayer", RpcTarget.All, collisionObject.gameObject);
                //RPCKillPlayer(collisionObject.gameObject);
            }
        }*/
        //Destroy(gameObject);
    }

    private void FixedUpdate() {
        //gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX ;  
        //gameObject.GetComponent<Rigidbody2D>();
    }

    //this video takes away some of its health corresponding to the projectile damage (here the can)
    //[PunRPC]
    private void DealDamageToPlayer() {
        //Character2DScript.userHealth -= projectileDamage;
    }


     public GameObject gameObject;
    //no knockback force on hanger
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
                    //pickUpWeapons.PickUp(gameObject);
                    
            collisionObject.gameObject.GetComponent<PickUpWeapons>().PickUp(gameObject);
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
                    //Debug.Log("GO" + gameObject);
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

    void Explode(Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float explosionRadius) {

        //var dir = (rb.transform.position2D - explosionPosition);
        //float wearoff = 1 - (dir.magnitude / explosionRadius);
        //rb.AddForce(dir.normalized * explosionForce * wearoff);
        
        //var explosionDir = rb.position - explosionPosition;
        //var explosionDistance = explosionDir.magnitude;
        //explosionDir /= explosionDistance;
        //add force in all directions
        //rb.AddForce(Mathf.Lerp(0, explosionForce, (1 - explosionDistance)) * explosionDir, ForceMode2D.Impulse);
        
        //or add an explosion radius within a radius 
    }

        //add animation

        //destroy grenadee
    
}
