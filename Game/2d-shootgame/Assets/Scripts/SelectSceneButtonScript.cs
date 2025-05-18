using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SelectSceneButtonScript : MonoBehaviour
{
    public Sprite buttonSprite;
    public Image imageBtn1;
    public Image imageBtn2;
    public Image imageBtn3;

    public int clearedLevel;
    private void Awake()
    {
        clearedLevel = PlayerPrefs.GetInt("clearedLevel");
        if(clearedLevel == 0)
        {
            imageBtn1.sprite = buttonSprite;
        }else if(clearedLevel <= 1)
        {
            imageBtn1.sprite = buttonSprite;
            imageBtn2.sprite = buttonSprite;
        }else if(clearedLevel >= 2)
        {
            imageBtn1.sprite = buttonSprite;
            imageBtn2.sprite = buttonSprite;
            imageBtn3.sprite = buttonSprite;
        }
    }

    public void GoToLevel1()
    {

        SceneManager.LoadScene("Level1");
        
    }

    public void GoToLevel2()
    {   
        if(clearedLevel >= 1)
        {
            SceneManager.LoadScene("Level2");
        }
    }
    public void GoToLevel3()
    {
        if(clearedLevel >= 2)
        {
            SceneManager.LoadScene("Level3");
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1.0f;
    }
}
