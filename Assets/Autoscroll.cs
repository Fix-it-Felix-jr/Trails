using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autoscroll : MonoBehaviour
{
    public float speed=0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed;

        var cameraPosition = Camera.main.gameObject.transform.position;
        cameraPosition.z += step;
        Camera.main.gameObject.transform.position = cameraPosition;
    }
}
