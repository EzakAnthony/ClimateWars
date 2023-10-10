using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;

public class Weapon : MonoBehaviour
{
    public float offset;    //offset on mouse curser
    //public CameraShake cameraShake; //reference to camera shake class
    private float magnitudeShake = 0.15f;    //magnitude of shake;
    private float shakeDuration = 0.2f;   //duration of shake
    public float chargingShotTime = 0f;    //how long user charged the shot for to be adaptive
    private float maxChargingPower = 5f;    //maximum power charging power (5 seconds)
    private PickUpWeapons pickUp;   //reference to pickupweapons class
    //private PhotonView PV;  //Photon view variable so that we can update so server

    //PickUpWeapons pickUpWeapons = new PickUpWeapons();
    private void Start() {
        pickUp = gameObject.GetComponent<PickUpWeapons>();  //instanciate the class on the player
        //PV = GetComponent<PhotonView>();    
    }
    
    // Update is called once per frame
    void Update()
    {
        //if it is my player we get the orientation based on the mouse inptu and register whether he pressed fire or not 
        //if (PV.IsMine) {
            OrientationWeapon();

            //check if user pressed on fire button which charges up the attack
            if (Input.GetButtonDown("Fire1")) {

                //Debug.Log("Can Throw : " + PickUpWeapons.canThrow); 
                //if we can throw a weapon
                if (PickUpWeapons.canThrow) {
                    //if we have not charged for more than the max power
                    if (chargingShotTime < maxChargingPower) {
                        chargingShotTime = chargingShotTime + Time.deltaTime;
                    }
                    //if we have charged beyond max charged power -> charging power is set to max
                    else {
                        chargingShotTime = maxChargingPower;
                        //also automatically shoot the bullet so that user can not always charge to the max without repercussions (skill gap)
                        //shoot the item we picked up from all the weapons that are available to pickup 
                        //PV.RPC("Shoot", RpcTarget.AllViaServer, chargingShotTime, PickUpWeapons.weaponToThrow);
                        Shoot(chargingShotTime, PickUpWeapons.weaponToThrow);
                        //StartCoroutine(cameraShake.Shake(shakeDuration, magnitudeShake));
                        chargingShotTime = 0f;
                    }
                }
            }

            //if user releases fire button , shoot and make screen shake
            if (Input.GetButtonUp("Fire1")) {
                //if we can throw the object and asked to shoot
                if (PickUpWeapons.canThrow) {
                    //release the weapon
                    //PV.RPC("Shoot", RpcTarget.AllViaServer, chargingShotTime, PickUpWeapons.weaponToThrow);
                    Shoot(chargingShotTime, PickUpWeapons.weaponToThrow);
                    //StartCoroutine(cameraShake.Shake(shakeDuration, magnitudeShake));
                    chargingShotTime = 0f;
                }
            }
       // }
    }

    //direction of player weapon/hand depending on the mouse input position
    private void OrientationWeapon() {
        //get direction of weapon, subtract mouse point to beginning of weapon
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //get rotation of weapon
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ + offset);
    }

    //shooting mechanic
    /*void Shoot(float charge, GameObject weaponToThrow) {
        Debug.Log("WEAPON TO THROW" + weaponToThrow.tag);
        //Unparent weapon rigidbody2d from parent
        PickUpWeapons.weaponsToPickUp[indexWeaponThrow].GetComponent<Rigidbody2D>().transform.parent = null;
        //weaponToThrow[indexWeaponThrow].GetComponent<Rigidbody2D>().transform.parent = null;
        //take away any constraints it may have
        //weaponToThrow[indexWeaponThrow].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        PickUpWeapons.weaponsToPickUp[indexWeaponThrow].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        //activate its collider
        PickUpWeapons.weaponsToPickUp[indexWeaponThrow].GetComponent<Collider2D>().enabled = true;
        //weaponToThrow[indexWeaponThrow].GetComponent<Collider2D>().enabled = true;
        //gather name of weaponToThrow in order to determine its velocity/ gravity
        //string typeOfWeapon = PickUpWeapons.weaponsToPickUp[indexWeaponThrow].name;
        string typeOfWeapon = PickUpWeapons.weaponsToPickUp[indexWeaponThrow].tag;
        
        //for can
        if(typeOfWeapon.Contains("Can")) {
            //set velocity to throw force and direction 
            PickUpWeapons.weaponsToPickUp[indexWeaponThrow].GetComponent<Rigidbody2D>().velocity = transform.right * CanWeapon.canSpeed * charge;
            //weaponToThrow[indexWeaponThrow].GetComponent<Rigidbody2D>().velocity = transform.right * Bullet.canSpeed * charge;
        }
        //for boomerang
        else if (typeOfWeapon.Contains("Boomerang")) {
            PickUpWeapons.weaponsToPickUp[indexWeaponThrow].GetComponent<Rigidbody2D>().velocity = transform.right * Hanger.hangerSpeed * charge;
        }
        //for glass bottle
        else if (typeOfWeapon.Contains("GlassBottle")) {
            PickUpWeapons.weaponsToPickUp[indexWeaponThrow].GetComponent<Rigidbody2D>().velocity = transform.right * GlassBottle.bottleSpeed * charge;
        }
        //for straw
        else if (typeOfWeapon.Contains("Straw")) {
            PickUpWeapons.weaponsToPickUp[indexWeaponThrow].GetComponent<Rigidbody2D>().velocity = transform.right * Straw.strawSpeed * charge;
        }
        //for bullet
        else if (typeOfWeapon.Contains("Bullet")) {
            PickUpWeapons.weaponsToPickUp[indexWeaponThrow].GetComponent<Rigidbody2D>().velocity = transform.right * Bullet.bulletSpeed * charge;
        }
        //for Spiked ball
        else if (typeOfWeapon.Contains("SpikedBall")) {
            PickUpWeapons.weaponsToPickUp[indexWeaponThrow].GetComponent<Rigidbody2D>().velocity = transform.right * Bullet.bulletSpeed * charge;
        }
        //reactivate its rigidbody
        //weaponToThrow[indexWeaponThrow].GetComponent<Rigidbody2D>().isKinematic = false;
        PickUpWeapons.weaponsToPickUp[indexWeaponThrow].GetComponent<Rigidbody2D>().isKinematic = false;
        //can again hold weapon as hand is free
        PickUpWeapons.canHold = true;
        //can not throw weapon as we do not hold any 
        PickUpWeapons.canThrow = false;
    }*/

    void Shoot(float charge, GameObject weaponToThrow) {
        //Unparent weapon rigidbody2d from parent
        weaponToThrow.GetComponent<Rigidbody2D>().transform.parent = null;
        //weaponToThrow[indexWeaponThrow].GetComponent<Rigidbody2D>().transform.parent = null;
        //take away any constraints it may have
        //weaponToThrow[indexWeaponThrow].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        weaponToThrow.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        //activate its collider
        weaponToThrow.GetComponent<Collider2D>().enabled = true;
        //weaponToThrow[indexWeaponThrow].GetComponent<Collider2D>().enabled = true;
        //gather name of weaponToThrow in order to determine its velocity/ gravity
        //string typeOfWeapon = PickUpWeapons.weaponsToPickUp[indexWeaponThrow].name;
        string typeOfWeapon = weaponToThrow.tag;
        
        //for can
        if(typeOfWeapon.Contains("Can")) {
            //set velocity to throw force and direction 
            weaponToThrow.GetComponent<Rigidbody2D>().velocity = transform.right * CanWeapon.canSpeed * charge;
            //weaponToThrow[indexWeaponThrow].GetComponent<Rigidbody2D>().velocity = transform.right * Bullet.canSpeed * charge;
        }
        //for boomerang
        else if (typeOfWeapon.Contains("Boomerang")) {
            weaponToThrow.GetComponent<Rigidbody2D>().velocity = transform.right * Hanger.hangerSpeed * charge;
        }
        //for glass bottle
        else if (typeOfWeapon.Contains("GlassBottle")) {
            weaponToThrow.GetComponent<Rigidbody2D>().velocity = transform.right * GlassBottle.bottleSpeed * charge;
        }
        //for straw
        else if (typeOfWeapon.Contains("Straw")) {
            weaponToThrow.GetComponent<Rigidbody2D>().velocity = transform.right * Straw.strawSpeed * charge;
        }
        //for bullet
        else if (typeOfWeapon.Contains("Bullet")) {
            weaponToThrow.GetComponent<Rigidbody2D>().velocity = transform.right * Bullet.bulletSpeed * charge;
        }
        //for Spiked ball
        else if (typeOfWeapon.Contains("SpikedBall")) {
            weaponToThrow.GetComponent<Rigidbody2D>().velocity = transform.right * Bullet.bulletSpeed * charge;
        }
        //reactivate its rigidbody
        //weaponToThrow[indexWeaponThrow].GetComponent<Rigidbody2D>().isKinematic = false;
        weaponToThrow.GetComponent<Rigidbody2D>().isKinematic = true;
        //can again hold weapon as hand is free
        PickUpWeapons.canHold = true;
        //can not throw weapon as we do not hold any 
        PickUpWeapons.canThrow = false;

        Debug.Log("Threw " + weaponToThrow.tag);
    }
}
