using UnityEngine;
using UnityEngine.Playables;

public class PlayOnce : MonoBehaviour
{
    PlayableDirector director; 

    void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.playOnAwake = false;
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("F", 0) == 0)
        {
            director.Play();
            PlayerPrefs.SetInt("F", 1);
        }
    }
}
