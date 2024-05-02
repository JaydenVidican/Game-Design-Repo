using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameSaveManager gameSave;
    public void NewGame()
    {
        gameSave.Reset();
        SceneManager.LoadScene("Room 2");
    }
    public void LoadGame()
    {
        gameSave.LoadScriptables();
        SceneManager.LoadScene("Room 2");
    }
    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void Menu()
    {
        SceneManager.LoadScene("Start");
        Time.timeScale = 1f;
    }
    public void QuitToDesktop()
    {
        Application.Quit();
    }
}
