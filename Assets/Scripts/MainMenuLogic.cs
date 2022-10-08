using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuLogic : MonoBehaviour
{
    //transition between scenes
    public void SelectBoard()
    {
        SceneManager.LoadScene("BoardSelectMenu");
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
