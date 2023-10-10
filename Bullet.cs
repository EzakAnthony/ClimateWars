using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static float bulletSpeed = 10f; //initial can speed if not charged (TBC)
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            ///pickUpWeapons.PickUp(gameObject);
            //if (PV.IsMine) {
            //    PV.RPC("PickUp", RpcTarget.AllViaServer, i);
            //}
            
            PickUpWeapons.weaponToThrow = gameObject;
            //can now throw the object
            PickUpWeapons.canThrow = true;
        }
        if (collisionObject.gameObject.tag == "Ground") {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f; //disable gravity on the objected collided into (IE, this one)
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero; //disable velocity on the objected collided into (IE, this one)
            gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0f; //disable velocity on the objected collided into (IE, this one)
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
                    
            collisionObject.gameObject.GetComponent<PickUpWeapons>().PickUp(gameObject);
            //pickUpWeapons.PickUp(gameObject);
            //if (PV.IsMine) {
            //    PV.RPC("PickUp", RpcTarget.AllViaServer, i);
            //}
            
            PickUpWeapons.weaponToThrow = gameObject;
            //can now throw the object
            PickUpWeapons.canThrow = true;
        }
    }
}
