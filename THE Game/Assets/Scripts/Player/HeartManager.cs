using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fHeart; //full heart
    public Sprite tqHeart; //three quarters heart
    public Sprite hHeart; //half heart
    public Sprite qHeart; //quarter heart
    public Sprite eHeart; //empty heart
    public FloatValue heartContainers;
    public FloatValue playerCurrentHealth;


    void Start()
    {
        heartContainers.initialValue = playerCurrentHealth.RuntimeValue / 4;
        InitHearts();
    }

    public void InitHearts()
    {
        for (int i = 0; i < heartContainers.initialValue; i++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fHeart;
        }
    }

     public void UpdateHearts() {
        float tempHealth = playerCurrentHealth.RuntimeValue / 4;
        for (int i = 0; i < heartContainers.initialValue; i++) {
            float currHeart = Mathf.Ceil(tempHealth - 1); //Mathf.Ceil rounds to nearest int
            if (i <= tempHealth - 1) 
            {
                //FullHeart
                hearts[i].sprite = fHeart;
            } else if (i >= tempHealth) 
            {
                //emptyHeart
                hearts[i].sprite = eHeart;
            } else if(i == currHeart &&  (tempHealth % 1) == .50)
            {
                //Half full heart
                hearts[i].sprite = hHeart;
            } else if (i == currHeart && (tempHealth % 1) == .25) 
            {
                //1/4 heart
                hearts[i].sprite = qHeart;
            } else /*(i == currHeart && (tempHealth % 1) == .75)*/ 
            {
                //3/4 heart
                hearts[i].sprite = tqHeart;
            }
        }
    }
} 
