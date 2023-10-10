using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SHOULD MIMICK A SPEAR
public class Straw : MonoBehaviour
{
    public static float strawSpeed = 10f; //initial straw speed if not charged (TBC)
    public float gravityEffect = 1f;    //gravoity effect on straw (try to mimic a true spear)
    public float strawDamage = 25f; //should correspond to speed and hitbox
    public Collider2D headCollider;

    // Start is called before the first frame update
    void Start()
    {
        //initialize its gravity
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityEffect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // only have trigger as no force applied to object as he dies right away
    private void OnTriggerEnter(Collider other) {
        //Debug.Log("JUST HIT" + other.gameObject.name);
        //if (collisionObject.gameObject.tag.Equals("Player") && !Character2DScript.isInvincible) {
            //if it is a head shot, instant kill (small hitbox big reward)
        //if we hit a player and he is not invicible (just dodged)
            //if (headCollider == other)
            //{
                //Kill player who took headshot
            //}
            //else {
                //take hin normal amount of damage
            //}
    }

     //when the can hits something, will take damage here afterwards and only take some knockback
    private void OnCollisionEnter2D(Collision2D collisionObject) {
        //Debug.Log("JUST HIT" + collisionObject.gameObject.name);
        //if we hit a player and he is not invicible (just dodged)
        //if (headCollider == collisionObject)
        /*
        if (collisionObject.gameObject.tag.Equals("Player") && !Character2DScript.isInvincible) {
            //if it is a head shot, instant kill (small hitbox big reward)
            
            //
            //deal damage to player accross network
            PV.RPC("RPCDealDamageToPlayer", RpcTarget.All);
            
            if (Character2DScript.userHealth <= 0) {
                PV.RPC("RPCKillPlayer", RpcTarget.All, collisionObject.gameObject);
                //RPCKillPlayer(collisionObject.gameObject);
            }
        }*/
    }

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
