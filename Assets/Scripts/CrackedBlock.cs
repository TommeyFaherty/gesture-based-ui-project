using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedBlock : MonoBehaviour
{
    private float thrust = 200.0f;
    private bool touched = false;
    private void OnCollisionEnter(Collision collision)
    {
        if(touched.Equals(false))
        {
            Debug.Log("Collision detected");
            StartCoroutine("CubeFalling");
            touched = true;
        }
    }
    IEnumerator CubeFalling(){ 
        yield return new WaitForSeconds(3.0f); 
        Debug.Log("wait", this); 
        transform.GetComponent<Rigidbody>().useGravity = true;
        transform.GetComponent<Rigidbody>().isKinematic = false;
        transform.localScale = new Vector3(transform.localScale.x * 0.95f, transform.localScale.x * 0.95f, 1);
        transform.GetComponent<Rigidbody>().AddForce(Vector3.down * thrust);
        Debug.Log("done", this); 

        FindObjectOfType<AudioManager>().Play("FallingFloor");
    } 
}
