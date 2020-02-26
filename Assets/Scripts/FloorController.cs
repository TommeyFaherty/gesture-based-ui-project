using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.I))
        {
            transform.Rotate(0,0.5f,0);
        }
        if(Input.GetKey(KeyCode.L))
        {
            transform.Rotate(0,0,0.5f);
        }
        if(Input.GetKey(KeyCode.J))
        {
            transform.Rotate(0.5f,0,0);
        }
    }
}
