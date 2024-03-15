using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    static MusicPlayer player = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        if (player != null)
            Destroy(gameObject);
        else
            player = this;
        GameObject.DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
