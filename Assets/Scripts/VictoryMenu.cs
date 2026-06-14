using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    public string firstLevelName = "Level_01";
    public string mainMenuName = "MainMenu";

    public void RestartGame()
    {
        SceneManager.LoadScene(firstLevelName);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(mainMenuName);
    }

    public void QuitGame()
    {
        Debug.Log("Saiu do jogo");
        Application.Quit();
    }
}