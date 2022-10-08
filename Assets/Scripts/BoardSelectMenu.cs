using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//When a board is selected, its scene must be loaded
public class BoardSelectMenu : MonoBehaviour
{
    public void FirstBoardSelected()
    {
        SceneManager.LoadScene("FirstBoard");
    }
    public void SecondBoardSelected()
    {
        SceneManager.LoadScene("SecondBoard");
    }
    public void ThirdBoardSelected()
    {
        SceneManager.LoadScene("ThirdBoard");
    }
    public void FourthBoardSelected()
    {
        SceneManager.LoadScene("FourthBoard");
    }
    public void FifthBoardSelected()
    {
        SceneManager.LoadScene("FifthBoard");
    }
    public void SixthBoardSelected()
    {
        SceneManager.LoadScene("SixthBoard");
    }
}
