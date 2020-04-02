using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject playBtn;
 
    public void PlayLevel1(){
        SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Single);
    }

    public void PlayLevel2(){
        SceneManager.LoadSceneAsync("Level2", LoadSceneMode.Single);
    }

    public void PlayLevel3(){
        SceneManager.LoadSceneAsync("Level3", LoadSceneMode.Single);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
