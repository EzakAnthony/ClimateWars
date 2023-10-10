using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//IT SHOULD MIMICK A BOOMERANG
public class Hanger : MonoBehaviour
{

    public float speed = 1f; // default speed 1 unit / second
    public float maxDistance = 5f; // default distance 5 units
    //private float distance = 0;
    public Transform boomerang; // the object you want to throw (assign from the scene)
    private float distance; // the distance it moves
    private bool isComingBack = false; // is it coming back
    public static float hangerSpeed = 10f; //initial hanger speed if not charged (TBC)
    private float gravityEffect = 1f;    //gravoity effect on straw (try to mimic a true spear)
    public float hangerDamage = 20f; //should correspond to speed and hitbox, can hit once on its way there and on its way back so (40 total)
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float travel = Time.deltaTime * speed;
        if (!isComingBack)
        {
            boomerang.Translate(Vector3.forward * travel); // moves object
            distance += travel; // update distance
            isComingBack = distance >= maxDistance; // goes back if distance reached
        }
        else
        {
            isComingBack = true;
            boomerang.Translate(Vector3.forward * -travel); // moves object
            distance -= travel; // update distance;
            enabled = distance > 0; // turning off when done
        }
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
