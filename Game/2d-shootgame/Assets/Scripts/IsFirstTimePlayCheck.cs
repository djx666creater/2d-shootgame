using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsFirstTimePlayCheck : MonoBehaviour
{
    private void Awake()
    {
        FirstTimePlayState();
    }
    public void FirstTimePlayState()
    {
        if (!PlayerPrefs.HasKey("IsFirstTimePlay"))             //初始化数据
        {
            PlayerPrefs.SetInt("IsFirstTimePlay", 1);
            PlayerPrefs.SetInt("PlayerBlood", 100);
            PlayerPrefs.SetInt("Bullet", 200);
            PlayerPrefs.SetInt("PlayerStone", 0);
            PlayerPrefs.SetInt("clearedLevel", 0);
        }
    }
}
