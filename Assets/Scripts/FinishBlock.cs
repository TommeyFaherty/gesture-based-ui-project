using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishBlock : MonoBehaviour
{
    public bool LastLevel = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (LastLevel)
            {
                // switch to "You Win" scene
                SceneManager.LoadSceneAsync("YouWin", LoadSceneMode.Single);
            }
            else
            {
                // switch to next level
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
