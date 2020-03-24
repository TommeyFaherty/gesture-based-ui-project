using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedBlock : MonoBehaviour
{
    private float thrust = 200.0f;
    private void OnCollisionEnter(Collision collision)
    {
       
        Debug.Log("Collision detected");
        StartCoroutine("CubeFalling");
        
    }
    IEnumerator CubeFalling(){ 
        yield return new WaitForSeconds(3.0f); 
        Debug.Log("wait", this); 
        transform.GetComponent<Rigidbody>().useGravity = true;
        transform.GetComponent<Rigidbody>().isKinematic = false;
        transform.GetComponent<Rigidbody>().AddForce(transform.up * thrust);
        Debug.Log("done", this); 
    } 
}
