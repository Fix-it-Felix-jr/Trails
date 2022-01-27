using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Teleport : MonoBehaviour
{
    public GameObject Oggetto;
    // Start is called before the first frame update
    void Start()
    {
        //Task.Delay(3000);
        //Telep(Oggetto, new Vector3(0,0,-50));
        //Camera.main.transform.position = new Vector3(0,0,-50);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter (Collider other)
    {
        Telep(Oggetto, new Vector3(0,0,-50));
        //if (other.tag == "Finish")
        //{
            //Camera.main.transform.position = new Vector3(0,0,-50);
            //transform.position = new Vector3(0,0,-50);
            //GetComponent<Camera>().main.transform.position = new Vector3 (0,0,-50);
        //}
    }
    void Telep (GameObject objectToTeleport, Vector3 coordinates) 
    { 
        objectToTeleport.transform.localPosition=coordinates; 
    } 
}
