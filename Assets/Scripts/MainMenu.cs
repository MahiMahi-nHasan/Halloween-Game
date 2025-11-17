using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "MainGame";
    public GameObject instructions;

    public void Play()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void Instructions()
    {
        instructions.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
