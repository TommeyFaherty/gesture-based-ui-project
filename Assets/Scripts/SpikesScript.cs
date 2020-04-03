using UnityEngine;
using UnityEngine.SceneManagement;

public class SpikesScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            // restart level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            FindObjectOfType<AudioManager>().Play("Spike");
        }
    }
}
