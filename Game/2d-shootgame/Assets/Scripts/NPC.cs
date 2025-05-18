using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour 
{
    public GameObject npcPanel;
    public Canvas m_Canvas;
    public Button m_Button;

    private bool canInteract; // 是否可以与NPC交互
    private bool isFirst;

    private void Awake()
    {
        canInteract = false;
        isFirst = true;
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.F) && isFirst)
        {
            isFirst = false;
            Time.timeScale = 0f;
            npcPanel.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))
        {
            canInteract = false;
        }
    }

    public void BuyButton()
    {
        int numStone = PlayerPrefs.GetInt("PlayerStone");
        int numBullet = PlayerPrefs.GetInt("Bullet");
        if(numStone >= 1000)
        {
            numStone -= 1000;
            numBullet += 100;
            PlayerPrefs.SetInt("PlayerStone", numStone);
            PlayerPrefs.SetInt("Bullet", numBullet);
            m_Canvas.StoneUpdate();
            m_Canvas.BulletUpdate();
        }
    }

    public void ReturnButton()
    {
        Time.timeScale = 1.0f;
        isFirst = true;
        npcPanel.SetActive(false);
    }
}
