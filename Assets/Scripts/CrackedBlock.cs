using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedBlock : MonoBehaviour
{
    private float thrust = 3.0f;
    private bool touched = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(touched.Equals(false))
        {
            // start falling coroutine
            StartCoroutine("CubeFalling");
            touched = true;
        }
    }
    IEnumerator CubeFalling(){ 
        // wait 3 seconds
        yield return new WaitForSeconds(3.0f);

        // make block fall
        transform.GetComponent<Rigidbody>().useGravity = true;
        transform.GetComponent<Rigidbody>().isKinematic = false;
        transform.localScale = new Vector3(transform.localScale.x * 0.93f, 1, transform.localScale.z * 0.93f);
        transform.GetComponent<Rigidbody>().velocity = -transform.up * thrust;

        // play sound effect
        FindObjectOfType<AudioManager>().Play("FallingFloor");
    } 
}
