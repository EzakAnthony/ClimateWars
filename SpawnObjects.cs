using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;
using System.IO;

public class SpawnObjects : MonoBehaviour
{
    //random spawn of objects
    public float spawnRateWeapons = 2f; // at what rate do new projectiles spawn
    private int whatWeaponToSpawn;  // what weapon to spawn next
    private float nextSpawnTime;    //when next weapon is coming up
    public float xMax = 4f;  //x coordinate of max spawn 
    private Vector2 whereToSpawn;    // where weapons will spawn
    ObjectPooler objectPooler;     //reference to pooler object
    //PhotonPooler photonPooler;  //reference to pooler photon
    private int numberOfWeapons;    //total number of weapons
    private string weaponTag;  //tag to represent which weapon we spawn
    //[SerializeField] private PhotonView PV;  //photon view reference
    private int seed;   //seed for random

    //instanciate
    private void Start() {
        //generate instance of the pool
        objectPooler = ObjectPooler.Instance;
        //photonPooler = PhotonPooler.Instance;
        //empty out weapons list
        
        //if (PhotonNetwork.IsMasterClient) {
        PickUpWeapons.weaponsToPickUp.Clear(); 
        //PV = GetComponent<PhotonView>();
        seed = 2;
        nextSpawnTime = Time.time + spawnRateWeapons;
        //}
        
        //PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void FixedUpdate() {
        //if we are master client we spawn weapons at a given rate from the pool
        //if (PhotonNetwork.IsMasterClient) {
            GenerateWeapons();
        //}
    }

    //generate weapons for all clients
    //[PunRPC]
    private void GenerateWeapons() {
        //on/ly spawn objects if it is time
        if (Time.time > nextSpawnTime) {
            //Instantiate(pool.prefab, new Vector2(0,0), new Quaternion(0,0,0,0));
            //PhotonNetwork.InstantiateSceneObject(Path.Combine("Prefab", "Bullet"), new Vector2(0,0), new Quaternion(0,0,0,0));
            //update next spawn time
            nextSpawnTime = Time.time + spawnRateWeapons;
            //where we spawn the weapons
            //Random.InitState(seed);
            //seed ++;
            float randomXPos = Random.Range(- xMax, xMax);
            whereToSpawn = new Vector2(randomXPos, transform.position.y);
            //randomize which weapon we spawn and get number of total weapons
            numberOfWeapons = ObjectPooler.Instance.weaponDictionnary.Count; //number of different weapons 
            //Debug.Log("Number of weapons " + numberOfWeapons);
            //numberOfWeapons = 2;
            whatWeaponToSpawn = Random.Range(1, numberOfWeapons + 1);
            //whatWeaponToSpawn
            //get tag of weapon that we spawn 
            switch(whatWeaponToSpawn) {
                //if number one we get bullet
                case 1: 
                weaponTag = "Can";
                //Debug.Log("Can To Spawn");
                break;
                //boomerang
                case 2:
                weaponTag = "GlassBottle";
                break;
                //bottle
                case 3:
                weaponTag = "Boomerang";
                break;
                //straw
                case 4:
                weaponTag = "Straw";
                break;            
                case 5:
                weaponTag = "Spiked Ball";
                break;
                case 6:
                weaponTag = "Bullet";
                break;
            }
            //generate specified object with tag
            GameObject weapon = ObjectPooler.Instance.SpawnFromPool(weaponTag, whereToSpawn, Quaternion.identity);
            //freeze its position on X only for all clients 
            //Debug.Log("weapon name" + weapon.name);
            FreezePositionX(weapon.name);
            //PV.RPC("FreezePositionX", RpcTarget.All, weapon.name);
            //weapon.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
            
            //PickUpWeapons.weaponsToPickUp.Add(weapon);
        }
    }
    
    //freeze X position on generated weapons for every client
    //[PunRPC]
    public void FreezePositionX(string goID) {
        GameObject weapon = GameObject.Find(goID);
        //freeze its position on X only
        weapon.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        //add weapon generated as a potentially picked up weapon
        PickUpWeapons.weaponsToPickUp.Add(weapon);
    }
}
