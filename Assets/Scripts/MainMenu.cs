using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject playBtn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
 
    public void QuitGame()
    {
        Debug.Log("Quit was pressed");
        Application.Quit();
    }
}
