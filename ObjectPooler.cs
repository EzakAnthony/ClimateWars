using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;
using System.IO;
//using Photon.Realtime;

//CLASS THAT GENERATES A POOL of Object specified with the gameobject prefab
public class ObjectPooler : MonoBehaviour {
    public Dictionary<string, Queue<GameObject>> weaponDictionnary;     //holds our weapon dictionary
    //Class but is more like a structure here that contains a tag, a prefab and a size
    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public List<Pool> pools;    //list containing a Pool
    
    //Singleton so that it can only be called once at a time
    #region  Singleton
    public static ObjectPooler Instance;

    //[SerializeField] private PhotonView PV;

    private void Awake() {
        if (ObjectPooler.Instance == null) {
            ObjectPooler.Instance = this;
        }
        else {
            if (ObjectPooler.Instance != this) {
                Destroy(ObjectPooler.Instance);
                ObjectPooler.Instance= this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
        
    }
    #endregion //end singleton 
    private void OnEnable() {
        //PV.GetComponent<PhotonView>();
    }
    private void Start() {
        
        //FOR PHOTON
        //PhotonNetwork.PrefabPool = this;
        //if (PhotonNetwork.IsMasterClient) {
        FillDictionnary();
        //}
    }

    public void  FillDictionnary() {
        //FOR PHOTON
        //PhotonNetwork.PrefabPool = this;

        //instantiate our dictionnary containing a string and a Queue of Gameobjects
        weaponDictionnary = new Dictionary<string, Queue<GameObject>>();
        // go through the list
        foreach(Pool pool in pools) {
            //instantiate a objectpool Queue
            Queue<GameObject> objectPool = new Queue<GameObject>();
            //while there are still objects to generate for pool to be filled
            for (int i = 0; i < pool.size; i++)
            {
                //rename the prefab name so they all have different names according to their indices and can be reused singularly for pickup
                string prefabName = pool.prefab.name;
                pool.prefab.name = prefabName + i.ToString();
                //instantiate the prefab we want from the specified pool
                //GameObject obj = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Bullet"), new Vector2(0,0), new Quaternion(0,0,0,0));
                GameObject obj = Instantiate(pool.prefab, new Vector2(0,0), new Quaternion(0,0,0,0));
                //disable it so we do not see it yet
                string objectID = obj.name;
                ChangeName(pool.prefab.name, prefabName);
                //PV.RPC("RPCChangeName", RpcTarget.AllBuffered, pool.prefab.name, prefabName);
                
                //Debug.Log("NAME OF OBJECT: " + objectID);
                Disable(objectID);
                //PV.RPC("RPCDisable", RpcTarget.AllBuffered, objectID);
                //put it in queue
                objectPool.Enqueue(obj);
                pool.prefab.name = prefabName;

            }
            //add the prefab and tag to the corresponding dictionnary 
            weaponDictionnary.Add(pool.tag, objectPool);
        }
        
    }

    //to disable gameobject and send it through network
    //[PunRPC]
    public void Disable(string goID) {
        GameObject go = GameObject.Find(goID);
        //go.name = goID;
        go.SetActive(false);
    }

    //[PunRPC]
    public void Enable(string goID) {
        //to find disabled object we must do this
        foreach (GameObject gameObj in Resources.FindObjectsOfTypeAll(typeof(GameObject))) 
        {
            //if(gameObj.name.Equals(goID)) {
            if (gameObj.name.Equals(goID)) {
                //gameObj.name = "HELLOOO";
                gameObj.SetActive(true);
                break;
            }
        }
    }

    //to change name of gameobject
    //[PunRPC]
    public void ChangeName(string goID, string prefabID) {
        foreach (GameObject gameObj in Resources.FindObjectsOfTypeAll(typeof(GameObject))) 
        {
            if (gameObj.name.Equals(goID)) {
                //to send it over netwrok
                gameObj.name = goID;
                break;
            }
            else if (gameObj.name.Equals(prefabID + "(Clone)")) {
                gameObj.name = goID + "(Clone)";
                break;
            }
        }
    }

    //spawn an object from the pool created
    public GameObject SpawnFromPool(string tag, Vector2 position, Quaternion rotation) {
        //make sure that the dictionnary asked for exists 
        if(!weaponDictionnary.ContainsKey(tag)){
            Debug.LogWarning("Pool with tag " + tag + " does not exist"); 
            return null; 
        }
        //dequeue an item so that we can get it
        GameObject objectToSpawn = weaponDictionnary[tag].Dequeue();
        //set it active so that it enters the scene
        string gameObjectID = objectToSpawn.name;
        Enable(gameObjectID);
        //ChangeName(gameObjectID);
        //PV.RPC("RPCEnable", RpcTarget.AllBuffered, gameObjectID);
        //PV.RPC("RPCChangeName", RpcTarget.AllBufferedViaServer, gameObjectID);
        //set its position and rotation to the specified in the function
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        weaponDictionnary[tag].Enqueue(objectToSpawn);
        //send object to spawn
        return objectToSpawn;
    }

}
