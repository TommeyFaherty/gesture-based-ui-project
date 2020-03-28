using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject playBtn;

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Single);
    }
 
    public void QuitGame()
    {
        Application.Quit();
    }
}
