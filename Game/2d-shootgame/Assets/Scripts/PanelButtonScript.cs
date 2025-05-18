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
        selectPanel.SetActive(true);           //�ؿ��е����ð�ť 
        Time.timeScale = 0f;
    }

    public void setSelectPanelOff()
    {
        selectPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void MainMenuPlayButton()        
    {
        SceneManager.LoadScene("Select");       //ѡ��ؿ��ĳ����ļ���
        Time.timeScale = 1.0f;
    }

    public void DataDeleteButton()
    {
        dataDeleteImage.SetActive(true);        //����ɾ����ɾ�����ݶԻ������
    }

    public void YesButton()
    {
        PlayerPrefs.DeleteAll();                //����Yes��ťɾ������
        IsFirstTimePlayCheck checkScript = GameObject.Find("IsFirstTimePlayCheck").GetComponent<IsFirstTimePlayCheck>();
        checkScript.FirstTimePlayState();       //��ʼ������
        dataDeleteImage.SetActive(false);
    }

    public void NoButton()
    {
        dataDeleteImage.SetActive(false);       //����no��ť����
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
