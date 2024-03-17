
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int c = 0;
    public void LoadLevel(string level)
    {
       SceneManager.LoadScene(level);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void counter()
    {
        c++;
        if (c == 5)
            LoadLevel("Win");
    }
//testing home commit
}
