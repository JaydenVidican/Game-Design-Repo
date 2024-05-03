using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransition : MonoBehaviour
{
    [Header("New Scene Variables")]
    public string sceneToLoad;
    public static string loadedScene;
    public Vector2 playerPosition;
    public VectorValue playerStorage;

    [Header("Transition Effects")]
    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;
    public float fadeWait;

    void Awake()
    {
        if (fadeInPanel != null)
        {
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
            Destroy(panel, 1);
        }
        
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerStorage.initialValue = playerPosition;
            StartCoroutine(FadeCo());
            //SceneManager.LoadScene(sceneToLoad);
        }
    }
    public IEnumerator FadeCo()
    {
        if (fadeOutPanel != null)
        {
            Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
            yield return new WaitForSeconds(fadeWait);
            GameObject gameSaveManager = GameObject.Find("GameSaveManager");
            //gameSaveManager.GetComponent<GameSaveManager>().RoomStore(sceneToLoad);
            AsyncOperation AsyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
            while(!AsyncOperation.isDone)
            {
                yield return null;
            }
        }
    }

    public void lose()
    {
        sceneToLoad = "Lose";
        SceneManager.LoadScene(sceneToLoad);
    }
}
