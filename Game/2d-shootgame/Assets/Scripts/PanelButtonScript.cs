using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PanelButtonScript : MonoBehaviour
{
    public GameObject selectPanel;
    public GameObject dataDeleteImage;

    private void Awake()
    {
    }
    public void setSelectPanelOn()
    {
        selectPanel.SetActive(true);           //关卡中的设置按钮 
        Time.timeScale = 0f;
    }

    public void setSelectPanelOff()
    {
        selectPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void MainMenuPlayButton()        
    {
        SceneManager.LoadScene("Select");       //选择关卡的场景的加载
        Time.timeScale = 1.0f;
    }

    public void DataDeleteButton()
    {
        dataDeleteImage.SetActive(true);        //按下删除，删除数据对话框出现
    }

    public void YesButton()
    {
        PlayerPrefs.DeleteAll();                //按下Yes按钮删除数据
        IsFirstTimePlayCheck checkScript = GameObject.Find("IsFirstTimePlayCheck").GetComponent<IsFirstTimePlayCheck>();
        checkScript.FirstTimePlayState();       //初始化数据
        dataDeleteImage.SetActive(false);
    }

    public void NoButton()
    {
        dataDeleteImage.SetActive(false);       //按下no按钮返回
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
